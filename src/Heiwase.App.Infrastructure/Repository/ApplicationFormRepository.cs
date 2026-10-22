using Heiwase.App.Infrastructure.Configuration;
using Heiwase.App.Infrastructure.DataAccess;
using Heiwase.App.Infrastructure.Entities;
using Heiwase.App.Infrastructure.Interface;

using Microsoft.Extensions.Options;

namespace Heiwase.App.Infrastructure.Repository;

internal sealed class ApplicationFormRepository : Repository<ApplicationFormEntity>, IApplicationFormRepository
{
    public ApplicationFormRepository(ICosmosContainerProvider containerProvider, IOptions<CosmosDbOptions> options)
        : base(containerProvider, options.Value.ApplicationsContainerName)
    {
    }

    public Task<IReadOnlyList<ApplicationFormEntity>> GetPendingAsync(CancellationToken cancellationToken = default)
        => QueryAsync(
            query => query.Where(form => !form.IsReviewed).OrderBy(form => form.SubmittedAt),
            cancellationToken);
}
