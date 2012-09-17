using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Data.Metadata.Edm;
using System.Data.Objects;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Company.DataAccessLayer.Infrastructure;

namespace Company.DataAccessLayer
{
    public class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private ICoreUnitOfWork unitOfWork;
        private ObjectContext objectContext;
        private IObjectSet<TEntity> dbSet;
        private ObservableCollection<TEntity> internalLocal;
        private bool detectChanges;
        private bool detectLocalChanges;
        private Type baseType;
        private EntitySetBase entitySet;

        public GenericRepository(ICoreUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            this.objectContext = this.unitOfWork.ObjectContext;
            this.dbSet = this.objectContext.CreateObjectSet<TEntity>();
            internalLocal = new ObservableCollection<TEntity>(
                    this.objectContext.ObjectStateManager
                    .GetObjectStateEntries(System.Data.EntityState.Unchanged
                    | System.Data.EntityState.Added
                    | System.Data.EntityState.Modified)
                    .Where(entry => entry.Entity is TEntity)
                    .Select(entry => (TEntity)entry.Entity));
            this.objectContext.ObjectStateManager.ObjectStateManagerChanged += OnObjectStateManagerChanged;
            this.internalLocal.CollectionChanged += OnInternalLocalChanged;
            this.detectChanges = true;
            this.detectLocalChanges = true;           

            var containerName = this.objectContext.DefaultContainerName;
            var container = this.unitOfWork.ObjectContext.MetadataWorkspace.GetEntityContainer(containerName, System.Data.Metadata.Edm.DataSpace.CSpace);
            this.entitySet = container.BaseEntitySets.Where(set => set.ElementType.Name.Equals(typeof(TEntity).Name)).FirstOrDefault();
            this.baseType = this.GetBaseType();
        }

        #region IRepository<TEntity> Members

        public IEnumerable<TEntity> Get(string includeProperties = "",
            Expression<Func<TEntity, bool>> filter = null, 
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, 
            int pageIndex = 1,
            int pageSize = int.MaxValue)
        {
            IQueryable<TEntity> query = this.dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {                
                query = orderBy(query);
            }

            return query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }

        public IQueryable<TEntity> GetQuery()
        {
            return this.dbSet;
        }       

        public System.Collections.ObjectModel.ObservableCollection<TEntity> Local
        {
            get 
            {
                return this.internalLocal;
            }
        }

        public IUnitOfWork UnitOfWork
        {
            get { return this.unitOfWork; }
        }

        public TEntity Single(Expression<Func<TEntity, bool>> predicate)
        {
            return this.dbSet.Single(predicate);
        }

        public TEntity First(Expression<Func<TEntity, bool>> predicate)
        {
            return this.dbSet.First(predicate);
        }

        public void Insert(TEntity entity)
        {
            this.dbSet.AddObject(entity);
        }

        public void Attach(TEntity entity)
        {
            this.dbSet.Attach(entity);
        }

        public TEntity Create()
        {
            return this.objectContext.CreateObject<TEntity>();
        }

        public TDerivedEntity Create<TDerivedEntity>() where TDerivedEntity : class, TEntity
        {
            return this.objectContext.CreateObject<TDerivedEntity>();
        }

        public void Delete(params object[] keyValues)
        {
            TEntity entityToDelete = this.Find(keyValues);
            Delete(entityToDelete);
        }

        public void Delete(TEntity entityToDelete)
        {
            if (entityToDelete == null)
            {
                throw new ArgumentNullException("entity");
            }
            this.objectContext.DeleteObject(entityToDelete);
        }

        public void Update(TEntity entityToUpdate)
        {
            var fqen = this.GetEntitySetName();

            object originalItem;
            EntityKey key = this.objectContext.CreateEntityKey(fqen, entityToUpdate);
            if (this.objectContext.TryGetObjectByKey(key, out originalItem))
            {
                this.objectContext.ApplyCurrentValues(key.EntitySetName, entityToUpdate);
            }
        }

        #endregion

        public TEntity Find(params object[] keyValues)
        {
            var key = GetEntityKey(keyValues);
            var obj = this.FindInStateManager(key) ?? this.FindInStore(key);
            if (obj != null && !(obj is TEntity))
                throw new ApplicationException("The entity with the given key values '" 
                    + string.Join(",", keyValues)
                    + "' could not be found or has the wrong type (found type:'" 
                    + obj.GetType().Name + "', searched type:'"
                    + typeof(TEntity).Name + "'");

            return (TEntity)obj;
        }

        private object FindInStateManager(EntityKey key)
        {
            ObjectStateEntry entry;
            if (key != null && this.objectContext.ObjectStateManager.TryGetObjectStateEntry(key, out entry))
                return entry.Entity;
            object obj = null;
            foreach (var objectStateEntry in this.objectContext.ObjectStateManager.GetObjectStateEntries(EntityState.Added)
                .Where(e => (!e.IsRelationship && e.Entity != null) ? this.baseType.IsAssignableFrom(e.Entity.GetType()) : false))
            {
                bool flag = true;
                foreach (var ekv in key.EntityKeyValues)
                {
                    int ordinal = objectStateEntry.CurrentValues.GetOrdinal(ekv.Key);
                    if (!KeyValuesEqual(ekv.Value, objectStateEntry.CurrentValues.GetValue(ordinal)))
                    {
                        flag = false;
                        break;
                    }

                    if (flag)
                    {
                        if (obj != null)
                            throw new ApplicationException("There are multiple entities matching the given key values in the state manager.");
                        obj = objectStateEntry.Entity;
                    }
                }
            }

            return obj;
        }

        private object FindInStore(EntityKey key)
        {
            if (key == null)
                return null;
            var stringBuilder = new System.Text.StringBuilder();
            stringBuilder.AppendFormat("SELECT VALUE X FROM {0} AS X WHERE ",
                string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}.{1}",
                QuoteIdentifier(this.entitySet.EntityContainer.Name), QuoteIdentifier(this.entitySet.Name)));

            EntityKeyMember[] entityKeyValues = key.EntityKeyValues;
            ObjectParameter[] objectParameterArray = new ObjectParameter[entityKeyValues.Length];
            for (int index = 0; index < entityKeyValues.Length; ++index)
            {
                if (index > 0)
                    stringBuilder.Append(" AND ");
                string name = string.Format(System.Globalization.CultureInfo.InvariantCulture, "p{0}", index.ToString(System.Globalization.CultureInfo.InvariantCulture));
                stringBuilder.AppendFormat("X.{0} = @{1}", QuoteIdentifier(entityKeyValues[index].Key), name);
                objectParameterArray[index] = new ObjectParameter(name, entityKeyValues[index].Value);
            }

            try
            {
                return this.objectContext.CreateQuery<TEntity>(stringBuilder.ToString(), objectParameterArray).SingleOrDefault();
            }
            catch (EntitySqlException ex)
            {
                throw new ArgumentException("The types of the given key values does not match the key value types of this entity '" + typeof(TEntity).Name + "'.", ex);
            }
        }        

        private EntityKey GetEntityKey(params object[] keyValues)
        {            
            if (this.entitySet == null)
            {
                throw new ApplicationException("The model does not contain an entity set containing an entity type '" + typeof(TEntity).Name + "'.");
            }

            var entitySetName = this.GetEntitySetName();
            var keyPropertyNames = entitySet.ElementType.KeyMembers.Select(km => km.ToString());
            if (keyPropertyNames.Count() != keyValues.Length)
                throw new ArgumentException("The given number of key values '" + keyValues.Length + "' does not match the number of key properties on the entity '" + keyPropertyNames.Count() + "'");

            var keyValuePairs = keyPropertyNames.Zip(keyValues, (name, value) => new KeyValuePair<string, object>(name, value));
            if (!keyValues.All(v => v != null))
                return null;

            return new EntityKey(entitySetName, keyValuePairs);
        }

        private void OnObjectStateManagerChanged(object sender, CollectionChangeEventArgs e)
        {
            if (!this.detectChanges)
            {
                this.detectChanges = true;
                return;
            }

            switch (e.Action)
            {
                case CollectionChangeAction.Add:
                    this.detectLocalChanges = false;
                    this.internalLocal.Add((TEntity)e.Element);
                    break;
                case CollectionChangeAction.Refresh:
                    this.internalLocal.Clear();
                    foreach (var newItem in this.objectContext.ObjectStateManager
                        .GetObjectStateEntries(System.Data.EntityState.Unchanged
                        | System.Data.EntityState.Added
                        | System.Data.EntityState.Modified)
                        .Where(entry => entry.Entity is TEntity)
                        .Select(entry => (TEntity)entry.Entity))
                    {
                        this.detectLocalChanges = false;
                        this.internalLocal.Add(newItem);
                    }
                    break;
                case CollectionChangeAction.Remove:
                    this.detectLocalChanges = false;
                    this.internalLocal.Remove((TEntity)e.Element);
                    break;
            }
        }

        private void OnInternalLocalChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (!this.detectLocalChanges)
            {
                this.detectLocalChanges = true;
                return;
            }

            if (e.Action == NotifyCollectionChangedAction.Remove
                || e.Action == NotifyCollectionChangedAction.Replace)
            {
                foreach (TEntity oldItem in e.OldItems)
                {
                    this.detectChanges = false;
                    this.objectContext.DeleteObject(oldItem);
                }
            }
            if (e.Action == NotifyCollectionChangedAction.Add
                || e.Action == NotifyCollectionChangedAction.Replace)
            {
                foreach (TEntity newItem in e.NewItems)
                {
                    this.detectChanges = false;
                    
                    ObjectStateEntry alreadyAddedEntry;
                    if(!(this.objectContext.ObjectStateManager.TryGetObjectStateEntry(newItem, out alreadyAddedEntry) &&
                        alreadyAddedEntry.State != EntityState.Deleted))

                    this.objectContext.AddObject(this.GetEntitySetName(), newItem);
                }
            }
        }

        private string GetEntitySetName()
        {
            if (this.entitySet == null)
            {
                throw new ApplicationException("The model does not contain a entity set containing an entity type '" + typeof(TEntity).Name + "'.");
            }
            return string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}.{1}", this.entitySet.EntityContainer.Name, this.entitySet.Name);
        }

        private Type GetBaseType()
        {
            var clrType = GetClrType(this.objectContext.MetadataWorkspace, this.entitySet.ElementType, new[] { typeof(TEntity).Assembly });
            return clrType.BaseType;
        }

        private static Type GetClrType(MetadataWorkspace metadataWorkspace, EdmType cSpaceEdmType, IEnumerable<Assembly> assemblies)
        {
            var collectionType = cSpaceEdmType as CollectionType;
            if (collectionType != null)
            {
                Type elementType = GetClrType(metadataWorkspace, collectionType.TypeUsage.EdmType, assemblies);

                return elementType;
            }

            var structuralType = cSpaceEdmType as StructuralType;
            if (structuralType != null)
            {
                var name = GetClrTypeName(metadataWorkspace, structuralType);
                foreach (var asm in assemblies)
                {
                    var clrType = asm.GetType(name);
                    if (clrType != null)
                    {
                        return clrType;
                    }
                }
            }

            var primitiveType = cSpaceEdmType as PrimitiveType;
            if (primitiveType != null)
            {
                return primitiveType.ClrEquivalentType;
            }

            return null;
        }

        private static string GetClrTypeName(MetadataWorkspace metadataWorkspace, StructuralType cSpaceType)
        {
            if (cSpaceType != null)
            {
                StructuralType oSpaceType;

                if (metadataWorkspace.TryGetObjectSpaceType(cSpaceType, out oSpaceType))
                {
                    // interesting note: oSpaceType is of type ClrType - an internal EF type that contains a EdmType to CLR type mapping...  
                    // so instead of getting the type name, we could go straight for the type  
                    // by doing: oSpaceType.GetProperty("ClrType",BindingFlags.Instance|BindingFlags.NonPublic).GetValue(oSpaceType, null);  
                    // but the classes are internal, so they might change and I don't want to touch them directly...
                    return oSpaceType.FullName;
                }
            }

            return null;
        }

        private static bool KeyValuesEqual(object x, object y)
        {
            if (x is DBNull)
                x = (object)null;
            if (y is DBNull)
                y = (object)null;
            if (object.Equals(x, y))
                return true;
            byte[] numArray1 = x as byte[];
            byte[] numArray2 = y as byte[];
            if (numArray1 == null || numArray2 == null || numArray1.Length != numArray2.Length)
                return false;
            for (int index = 0; index < numArray1.Length; ++index)
            {
                if ((int)numArray1[index] != (int)numArray2[index])
                    return false;
            }
            return true;
        }

        private static string QuoteIdentifier(string identifier)
        {
            return "[" + identifier.Replace("]", "]]") + "]";
        }

    }
}
