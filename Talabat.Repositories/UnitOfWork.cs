using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.RepositoriesInterfaces;
using Talabat.Repositories.Data;
using Talabat.Repositories.Repositories;

namespace Talabat.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreDBContext _context;
        private Hashtable _Repositories;

        public UnitOfWork(StoreDBContext context) {
            _context = context;
            _Repositories = new Hashtable();
        }
        public async Task<int> CompleteAsync() => await _context.SaveChangesAsync();

        public async ValueTask DisposeAsync()=>await _context.DisposeAsync();

        public IGenericRepository<TEntity> Repositories<TEntity>() where TEntity : BaseEntity
        {
            var type = typeof(TEntity).Name;
            if (!_Repositories.ContainsKey(type))
            {
            var repo = new GenericRepositories<TEntity>(_context);
            _Repositories.Add(type,repo);
            }

            return _Repositories[type] as IGenericRepository<TEntity>;


        }
    }
}
