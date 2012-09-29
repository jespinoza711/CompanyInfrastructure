using System.Linq;
using Company.Infrastructure.DataAccessLayer.Specification;

namespace Company.DataAccessLayer.Specification
{
    /// <summary>
    /// http://devlicio.us/blogs/jeff_perrin/archive/2006/12/13/the-specification-pattern.aspx
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class CompositeSpecification<TEntity> : ISpecification<TEntity> where TEntity : class
    {
        protected readonly Specification<TEntity> leftSide;
        protected readonly Specification<TEntity> rightSide;

        public CompositeSpecification(Specification<TEntity> leftSide, Specification<TEntity> rightSide)
        {
            this.leftSide = leftSide;
            this.rightSide = rightSide;
        }

        public abstract TEntity SatisfyingEntityFrom(IQueryable<TEntity> query);

        public abstract IQueryable<TEntity> SatisfyingEntitiesFrom(IQueryable<TEntity> query);
    }
}
