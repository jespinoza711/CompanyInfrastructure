using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Company.Infrastructure.DataAccessLayer
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IEnumerable<TEntity> Get(string includeProperties = "",
            Expression<Func<TEntity, bool>> filter = null, 
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, 
            int pageIndex = 1,
            int pageSize = int.MaxValue);

        TEntity Find(params object[] keyValues);

        IQueryable<TEntity> GetQuery();

        System.Collections.ObjectModel.ObservableCollection<TEntity> Local { get; }

        TEntity Single(Expression<Func<TEntity, bool>> predicate);

        TEntity First(Expression<Func<TEntity, bool>> predicate);

        void Insert(TEntity entity);

        void Attach(TEntity entity);

        TEntity Create();

        TDerivedEntity Create<TDerivedEntity>() where TDerivedEntity : class, TEntity;

        void Delete(params object[] keyValues);

        void Delete(TEntity entityToDelete);

        void Update(TEntity entityToUpdate);

        IUnitOfWork UnitOfWork { get; }
    }
}
