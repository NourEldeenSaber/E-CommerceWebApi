using Domain.Contracts;
using Domain.Entities;
using System.Linq.Expressions;

namespace Services.Specifications
{
    internal abstract class BaseSpecifications<TEntity, TKey>
        : ISpecifications<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        #region Condition(Where) Criteria
        
        protected BaseSpecifications(Expression<Func<TEntity, bool>>? criteria)
        {
            Criteria = criteria;
        }
        public Expression<Func<TEntity, bool>>? Criteria { get; private set; } 
        
        #endregion

        #region Includes

        public List<Expression<Func<TEntity, object>>> IncludeExpressions { get; } = new();

        // Add Includes 
        protected void AddIncludes(Expression<Func<TEntity, object>> IncludeExpression)
        {
            IncludeExpressions.Add(IncludeExpression);
        }

        #endregion


        #region Sorting [OrderBy,OrderByDescending]

        public Expression<Func<TEntity, object>> OrderBy { get; private set; }
        
        public Expression<Func<TEntity, object>> OrderByDescending { get; private set; }

        protected void AddOrderBy(Expression<Func<TEntity, object>> orderByExpression)
        =>  OrderBy = orderByExpression;

        protected void AddOrderByDescending(Expression<Func<TEntity, object>> orderByDescendingExpression)
        => OrderBy = orderByDescendingExpression;


        #endregion
    }
}
