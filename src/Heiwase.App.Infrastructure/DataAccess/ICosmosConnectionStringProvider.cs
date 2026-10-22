namespace Heiwase.App.Infrastructure.DataAccess;

public interface ICosmosConnectionStringProvider
{
    Task<string> GetConnectionStringAsync(CancellationToken cancellationToken);
}
