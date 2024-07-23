using Talabat.Core.Entities;
using Talabat.Core.RepositoriesInterfaces;

namespace Talabat.Core
{
    public interface IUnitOfWork:IAsyncDisposable
    {
        Task<int> CompleteAsync();
        IGenericRepository<TEntity> Repositories<TEntity>() where TEntity : BaseEntity;
    }
}
