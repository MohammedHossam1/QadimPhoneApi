using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.RepositoriesInterfaces;
using Talabat.Core.Specification;
using Talabat.Repositories.Data;
using Talabat.Repositories.Specification;

namespace Talabat.Repositories.Repositories
{
    public class GenericRepositories<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreDBContext _context;

        public GenericRepositories(StoreDBContext context)
        {
            _context = context;
        }
        public  async Task<T?> GetAsync(int id)
        {
            if (typeof(T)==typeof(Product))
            {
                return  await _context.Products.Where(p=>p.Id==id).Include(P=>P.ProductType).FirstOrDefaultAsync() as T;
            }
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            if (typeof(T) == typeof(Product))
            {
                return (IReadOnlyList<T>)await _context.Products.Include(P => P.ProductType).ToListAsync();
            }
            return await _context.Set<T>().ToListAsync();
        }



        public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T>spec)
        {
         return await ApplySpecifications(spec).ToListAsync();
        }
        public  Task<T?> GetWithSpecAsync(ISpecification<T> spec)
        {
            return ApplySpecifications(spec).FirstOrDefaultAsync();
        }

        private IQueryable<T> ApplySpecifications(ISpecification<T> spec)
        {
            return  SpecificationEvaluator<T>.getQuery(_context.Set<T>(), spec);
        }
        



        public  async Task<int> GetCountAsync(ISpecification<T> spec)
        {
            return await ApplySpecifications(spec).CountAsync();
        }

        public async Task AddAsync(T item) => await _context.Set<T>().AddAsync(item);

        public void Delete(T item)
        {
            _context.Set<T>().Remove(item);
        }

        public void Update(T item)
        {
            _context.Set<T>().Update(item);
        }

    }
}
