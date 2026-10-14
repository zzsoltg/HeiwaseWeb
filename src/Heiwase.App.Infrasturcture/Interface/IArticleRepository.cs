using Heiwase.App.Infrastructure.Entities;

namespace Heiwase.App.Infrastructure.Interface;

public interface IArticleRepository : IRepository<Article>
{
    /// <summary>Returns published articles, newest first.</summary>
    Task<IReadOnlyList<Article>> GetPublishedAsync(CancellationToken cancellationToken = default);
}
