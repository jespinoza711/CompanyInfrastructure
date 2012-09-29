using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Company.Infrastructure.DataAccessLayer.Specification
{
    public interface ISpecification<TEntity> where TEntity : class
    {
        TEntity SatisfyingEntityFrom(IQueryable<TEntity> query);

        IQueryable<TEntity> SatisfyingEntitiesFrom(IQueryable<TEntity> query);
    }
}
