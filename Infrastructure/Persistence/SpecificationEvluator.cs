using Domain.Contracts;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Presistence
{
    internal static class SpecificationEvluator
    {
        public static IQueryable<TEntity> CreateQuery<TEntity,TKey>(IQueryable<TEntity> inputQuery ,
            ISpecifications<TEntity, TKey> specifications) where TEntity : BaseEntity<TKey>
        {
            var query = inputQuery;

            //Where
            if (specifications.Criteria is not null)
                query = query.Where(specifications.Criteria);

            //Includes
            if (specifications.IncludeExpressions is not null && specifications.IncludeExpressions.Count > 0)
            {
                query = specifications.IncludeExpressions
                    .Aggregate(query, (currentQuery, expression) => currentQuery.Include(expression));
            }

            return query;
        }
    }
}
