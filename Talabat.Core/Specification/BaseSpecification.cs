using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specification
{
    public class BaseSpecification<T> : ISpecification<T> where T : BaseEntity
    {
        public Expression<Func<T, bool>> wherewor { get; set; } = null;
        public List<Expression<Func<T, object>>> Includat { get; set; } = new List<Expression<Func<T, object>>>();
        public Expression<Func<T, object>> OrderBy { get; set; } = null;
        public Expression<Func<T, object>> OrderByDesc { get; set; } = null;
        public int Skip { get ; set; }
        public int Take { get;set; }
        public bool IsPagination { get;set; }

        public BaseSpecification()
        {
            
        }
        public BaseSpecification(Expression<Func<T, bool>> whereExpression)
        {
            wherewor= whereExpression;
            
        }

        public void addOrderBy(Expression<Func<T, object>> expression)
        {
            OrderBy = expression;
        }
        public void addOrderByDesc(Expression<Func<T, object>> expression)
        {
            OrderByDesc = expression;
        }
    
        public void addPagination(int skip,int take)
        {
            IsPagination = true;
            Skip = skip;
            Take = take;
        }
    }
}
