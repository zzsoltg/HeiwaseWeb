using Heiwase.App.Infrastructure.Configuration;
using Heiwase.App.Infrastructure.DataAccess;
using Heiwase.App.Infrastructure.Entities;
using Heiwase.App.Infrastructure.Interface;

using Microsoft.Extensions.Options;

namespace Heiwase.App.Infrastructure.Repository;

internal sealed class HallOfFameMemberRepository : Repository<HallOfFameMemberEntity>, IHallOfFameMemberRepository
{
    public HallOfFameMemberRepository(ICosmosContainerProvider containerProvider, IOptions<CosmosDbOptions> options)
        : base(containerProvider, options.Value.HallOfFameContainerName)
    {
    }

    public Task<IReadOnlyList<HallOfFameMemberEntity>> GetOrderedAsync(CancellationToken cancellationToken = default)
        => QueryAsync(query => query.OrderBy(member => member.Order), cancellationToken);
}
