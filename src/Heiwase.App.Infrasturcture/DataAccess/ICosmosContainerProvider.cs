using Microsoft.Azure.Cosmos;

namespace Heiwase.App.Infrastructure.DataAccess;

public interface ICosmosContainerProvider
{
    Task<Container> GetContainerAsync(string containerName, CancellationToken cancellationToken);
}
