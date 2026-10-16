using Heiwase.App.Infrastructure.Entities;

namespace Heiwase.App.Infrastructure.Interface;

public interface IArticleRepository : IRepository<ArticleEntity>
{
    /// <summary>Returns published articles, newest first.</summary>
    Task<IReadOnlyList<ArticleEntity>> GetPublishedAsync(CancellationToken cancellationToken = default);
}
