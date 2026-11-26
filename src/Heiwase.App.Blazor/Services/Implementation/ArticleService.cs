using System.Net;
using System.Net.Http.Json;
using Heiwase.App.Blazor.Services.Interface;
using Heiwase.App.Shared.Models;

namespace Heiwase.App.Blazor.Services.Implementation;

public sealed class ArticleService(HttpClient http) : IArticleService
{
    private const string Endpoint = "api/articles";

    public async Task<IReadOnlyList<ArticleDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        using var response = await http.GetAsync(Endpoint, cancellationToken);
        response.EnsureSuccessStatusCode();
        var articles = await response.Content.ReadFromJsonAsync<List<ArticleDto?>>(cancellationToken)
            ?? throw new InvalidDataException("The article list response was null.");
        return [.. articles.Select(RequireValid)];
    }

    public async Task<ArticleDto?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        using var response = await http.GetAsync($"{Endpoint}/{Uri.EscapeDataString(id)}", cancellationToken);
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();
        var article = await response.Content.ReadFromJsonAsync<ArticleDto>(cancellationToken);
        return RequireValid(article);
    }

    private static ArticleDto RequireValid(ArticleDto? article)
    {
        if (article is null || string.IsNullOrWhiteSpace(article.Id)
            || string.IsNullOrWhiteSpace(article.Title) || article.MarkdownContent is null)
        {
            throw new InvalidDataException("The article response contains invalid fields.");
        }

        return article;
    }
}
