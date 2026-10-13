using Heiwase.App.Infrastructure.Interface;

namespace Heiwase.App.Infrastructure.Repository;

internal class Repository<T> : IRepository<T> where T : class
{
    public Task AddAsync(T entity)
    {
        throw new NotImplementedException( );
    }

    public Task DeleteAsync(T entity)
    {
        throw new NotImplementedException( );
    }

    public Task<IEnumerable<T>> GetAllAsync( )
    {
        throw new NotImplementedException( );
    }

    public Task<T?> GetByIdAsync(string id)
    {
        throw new NotImplementedException( );
    }

    public Task UpdateAsync(T entity)
    {
        throw new NotImplementedException( );
    }
}
