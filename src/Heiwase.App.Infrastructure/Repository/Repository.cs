using System.Net;

using Heiwase.App.Infrastructure.DataAccess;
using Heiwase.App.Infrastructure.Entities;
using Heiwase.App.Infrastructure.Interface;

using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;

namespace Heiwase.App.Infrastructure.Repository;

/// <summary>
/// Generic Cosmos DB repository. All containers are partitioned by <c>/id</c>, so the entity id doubles as the partition key.
/// </summary>
internal abstract class Repository<T> : IRepository<T> where T : BaseEntity
{
    private readonly ICosmosContainerProvider _containerProvider;
    private readonly string _containerName;

    protected Repository(ICosmosContainerProvider containerProvider, string containerName)
    {
        ArgumentNullException.ThrowIfNull(containerProvider);
        ArgumentException.ThrowIfNullOrWhiteSpace(containerName);

        _containerProvider = containerProvider;
        _containerName = containerName;
    }

    public async Task<T?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);

        Container container = await GetContainerAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            ItemResponse<T> response = await container
                .ReadItemAsync<T>(id, new PartitionKey(id), cancellationToken: cancellationToken)
                .ConfigureAwait(false);

            return response.Resource;
        }
        catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }
    }

    public Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default)
        => QueryAsync(query => query, cancellationToken);

    public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);

        if (string.IsNullOrWhiteSpace(entity.Id))
        {
            entity.Id = Guid.NewGuid().ToString();
        }

        Container container = await GetContainerAsync(cancellationToken).ConfigureAwait(false);
        await container
            .CreateItemAsync(entity, new PartitionKey(entity.Id), cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);
        ArgumentException.ThrowIfNullOrWhiteSpace(entity.Id);

        Container container = await GetContainerAsync(cancellationToken).ConfigureAwait(false);
        await container
            .ReplaceItemAsync(entity, entity.Id, new PartitionKey(entity.Id), cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);
        ArgumentException.ThrowIfNullOrWhiteSpace(entity.Id);

        Container container = await GetContainerAsync(cancellationToken).ConfigureAwait(false);
        await container
            .DeleteItemAsync<T>(entity.Id, new PartitionKey(entity.Id), cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    protected async Task<IReadOnlyList<T>> QueryAsync(
        Func<IQueryable<T>, IQueryable<T>> buildQuery,
        CancellationToken cancellationToken)
    {
        Container container = await GetContainerAsync(cancellationToken).ConfigureAwait(false);
        IQueryable<T> query = buildQuery(container.GetItemLinqQueryable<T>());

        List<T> results = [];
        using FeedIterator<T> iterator = query.ToFeedIterator();
        while (iterator.HasMoreResults)
        {
            FeedResponse<T> page = await iterator.ReadNextAsync(cancellationToken).ConfigureAwait(false);
            results.AddRange(page);
        }

        return results;
    }

    private Task<Container> GetContainerAsync(CancellationToken cancellationToken)
        => _containerProvider.GetContainerAsync(_containerName, cancellationToken);
}
