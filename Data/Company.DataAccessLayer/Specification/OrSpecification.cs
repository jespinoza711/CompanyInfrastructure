using System.Linq;
using Company.DataAccessLayer.Extensions;

namespace Company.DataAccessLayer.Specification
{
    public class OrSpecification<TEntity> : CompositeSpecification<TEntity> where TEntity : class
    {
        public OrSpecification(Specification<TEntity> leftSide, Specification<TEntity> rightSide)
            : base(leftSide, rightSide)
        {
        }

        public override TEntity SatisfyingEntityFrom(IQueryable<TEntity> query)
        {
            return SatisfyingEntitiesFrom(query).FirstOrDefault();
        }

        public override IQueryable<TEntity> SatisfyingEntitiesFrom(IQueryable<TEntity> query)
        {
            return query.Where(leftSide.Predicate.Or(rightSide.Predicate));
        }
    }
}
