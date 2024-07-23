using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specification;

namespace Talabat.Core.RepositoriesInterfaces
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        public Task<T?> GetAsync(int id);
        public Task<IReadOnlyList<T>> GetAllAsync();

        public Task<T?> GetWithSpecAsync(ISpecification<T> spec);
        public Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> spec);
        public Task<int> GetCountAsync(ISpecification<T> spec);

        public Task AddAsync(T item);
        public void Delete(T item);
        public void Update(T item);


    }
}
