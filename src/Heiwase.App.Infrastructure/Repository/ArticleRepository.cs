using Heiwase.App.Infrastructure.Configuration;
using Heiwase.App.Infrastructure.DataAccess;
using Heiwase.App.Infrastructure.Entities;
using Heiwase.App.Infrastructure.Interface;

using Microsoft.Extensions.Options;

namespace Heiwase.App.Infrastructure.Repository;

internal sealed class ArticleRepository : Repository<ArticleEntity>, IArticleRepository
{
    public ArticleRepository(ICosmosContainerProvider containerProvider, IOptions<CosmosDbOptions> options)
        : base(containerProvider, options.Value.ArticlesContainerName)
    {
    }

    public Task<IReadOnlyList<ArticleEntity>> GetPublishedAsync(CancellationToken cancellationToken = default)
        => QueryAsync(
            query => query.Where(article => article.IsPublished).OrderByDescending(article => article.CreatedAt),
            cancellationToken);
}
