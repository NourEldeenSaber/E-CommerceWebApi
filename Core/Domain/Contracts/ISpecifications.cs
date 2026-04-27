using Domain.Entities;
using System.Linq.Expressions;

namespace Domain.Contracts
{
    public interface ISpecifications<TEntity,TKey> where TEntity : BaseEntity<TKey>
    {
        // Signature for property [Expression => Where]
        public Expression<Func<TEntity,bool>>? Criteria { get;  }

        // Signature for property [Expression => Include]
        // ex => .Include(p => p.RelatedData).Include(...);
        public List<Expression<Func<TEntity,object>>> IncludeExpressions { get; }

        //OrderBy ,OrderByDes
        public Expression<Func<TEntity,object>> OrderBy { get;  }
        public Expression<Func<TEntity,object>> OrderByDescending { get; }

        //Pagination [skip,take]
        public int Skip { get; }
        public int Take { get; }
        public bool IsPginated { get; }

    }
}
