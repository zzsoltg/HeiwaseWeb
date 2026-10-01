using Heiwase.App.Infrasturcture.Interface;

namespace Heiwase.App.Infrasturcture.Repository;

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

    public Task<T?> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException( );
    }

    public Task UpdateAsync(T entity)
    {
        throw new NotImplementedException( );
    }
}
