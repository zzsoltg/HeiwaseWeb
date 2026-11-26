using Heiwase.App.Shared.Models;

namespace Heiwase.App.Blazor.Services.Interface;

public interface IArticleService
{
    Task<IReadOnlyList<ArticleDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ArticleDto?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
}
