using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specification;

namespace Talabat.Repositories.Specification
{
    public class SpecificationEvaluator<TEntity> where TEntity : BaseEntity
    {
        public static  IQueryable<TEntity> getQuery(IQueryable<TEntity> inputQuery,ISpecification<TEntity> spec)
        {
            var query = inputQuery;
            if (spec.wherewor is not null)
            {
                query = query.Where(spec.wherewor);
            }
            if (spec.OrderBy is not null)
            {
                query = query.OrderBy(spec.OrderBy);
            }
            if (spec.OrderByDesc is not null)
            {
                query = query.OrderByDescending(spec.OrderByDesc);
            }
            if (spec.IsPagination)
            {
                if (spec.Skip < 0)
                {
                    spec.Skip = 0; // Set to 0 if negative (or handle differently based on your business logic)
                }
                query = query.Skip(spec.Skip).Take(spec.Take);
            }
            query = spec.Includat.Aggregate(query, (currentQuery, includeExepressin) => currentQuery.Include(includeExepressin));
                return query;
        }
    }
}
