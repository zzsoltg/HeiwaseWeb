using System.Text.Json;

using Heiwase.App.Infrastructure.Configuration;

using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;

namespace Heiwase.App.Infrastructure.DataAccess;

/// <summary>
/// Owns the single, lazily created <see cref="CosmosClient"/> (the SDK recommends one instance per application).
/// </summary>
internal sealed class CosmosContainerProvider : ICosmosContainerProvider, IDisposable
{
    private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web);

    private readonly ICosmosConnectionStringProvider _connectionStringProvider;
    private readonly string _databaseName;
    private readonly SemaphoreSlim _initializationLock = new(1, 1);
    private CosmosClient? _client;

    public CosmosContainerProvider(ICosmosConnectionStringProvider connectionStringProvider, IOptions<CosmosDbOptions> options)
    {
        ArgumentNullException.ThrowIfNull(connectionStringProvider);
        ArgumentNullException.ThrowIfNull(options);

        _connectionStringProvider = connectionStringProvider;
        _databaseName = options.Value.DatabaseName;
    }

    public async Task<Container> GetContainerAsync(string containerName, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(containerName);

        var client = await GetClientAsync(cancellationToken).ConfigureAwait(false);
        return client.GetContainer(_databaseName, containerName);
    }

    public void Dispose()
    {
        _client?.Dispose();
        _initializationLock.Dispose();
    }

    private async Task<CosmosClient> GetClientAsync(CancellationToken cancellationToken)
    {
        if (_client is not null)
        {
            return _client;
        }

        await _initializationLock.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            if (_client is null)
            {
                var connectionString = await _connectionStringProvider
                    .GetConnectionStringAsync(cancellationToken)
                    .ConfigureAwait(false);

                _client = new CosmosClient(connectionString, new CosmosClientOptions
                {
                    UseSystemTextJsonSerializerWithOptions = SerializerOptions,
                    ApplicationName = "Heiwase.App",
                });
            }

            return _client;
        }
        finally
        {
            _initializationLock.Release();
        }
    }
}
