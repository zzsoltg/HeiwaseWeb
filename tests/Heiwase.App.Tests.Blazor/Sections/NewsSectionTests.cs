using Bunit;
using Heiwase.App.Blazor.Components.Sections.Home.NewsSection;
using Heiwase.App.Blazor.Services.Implementation;
using Heiwase.App.Blazor.Services.Interface;
using Heiwase.App.Shared.Models;
using Heiwase.App.Tests.Blazor.Support;
using Microsoft.Extensions.DependencyInjection;

namespace Heiwase.App.Tests.Blazor.Sections;

public sealed class NewsSectionTests : SiteTestContext
{
    [Fact]
    public void ShowsLoadingThenSortedArticlesWithLinksAndPlainText()
    {
        var source = new FakeArticles();
        Services.AddSingleton<IArticleService>(source);
        Services.AddSingleton<IArticleMarkdownRenderer, ArticleMarkdownRenderer>();
        var cut = Render<NewsSection>();
        Assert.NotEmpty(cut.FindAll("[role=status]"));

        source.List.SetResult([
            new() { Id = "old", Title = "Old", MarkdownContent = "Earlier", CreatedAt = new DateTime(2025, 1, 1) },
            new() { Id = "new", Title = "New", MarkdownContent = "Hello **world**", CreatedAt = new DateTime(2026, 1, 1) }
        ]);
        cut.WaitForAssertion(() => Assert.Equal("New", cut.Find(".news-card h3").TextContent));
        Assert.Equal("/news/new", cut.Find(".news-card").GetAttribute("href"));
        Assert.Contains("Hello world", cut.Find(".news-card").TextContent);
        Assert.Empty(cut.FindAll(".news-card strong"));
    }

    [Fact]
    public void ShowsEmptyState()
    {
        var source = new FakeArticles();
        source.List.SetResult([]);
        Services.AddSingleton<IArticleService>(source);
        Services.AddSingleton<IArticleMarkdownRenderer, ArticleMarkdownRenderer>();
        var cut = Render<NewsSection>();
        cut.WaitForAssertion(() => Assert.NotEmpty(cut.FindAll(".news-empty")));
    }

    [Fact]
    public void ReportsFailedRequestRatherThanAnEmptyFeed()
    {
        var source = new FakeArticles();
        source.List.SetException(new HttpRequestException("offline"));
        Services.AddSingleton<IArticleService>(source);
        Services.AddSingleton<IArticleMarkdownRenderer, ArticleMarkdownRenderer>();
        var cut = Render<NewsSection>();
        cut.WaitForAssertion(() => Assert.NotEmpty(cut.FindAll("[role=alert]")));
        Assert.Empty(cut.FindAll(".news-empty"));
    }

    [Fact]
    public void RetryAfterFailureLoadsArticles()
    {
        Services.AddSingleton<IArticleService>(new RetryArticles());
        Services.AddSingleton<IArticleMarkdownRenderer, ArticleMarkdownRenderer>();
        var cut = Render<NewsSection>();
        cut.Find(".news-retry").Click();
        cut.WaitForAssertion(() => Assert.Equal("Recovered", cut.Find(".news-card h3").TextContent));
    }

    [Fact]
    public void EscapesArticleTitleAndLocalizesLabels()
    {
        UseCulture(Hungarian);
        var source = new FakeArticles();
        source.List.SetResult([new() { Id = "a", Title = "<img src=x onerror=alert(1)>", MarkdownContent = "Text" }]);
        Services.AddSingleton<IArticleService>(source);
        Services.AddSingleton<IArticleMarkdownRenderer, ArticleMarkdownRenderer>();
        var cut = Render<NewsSection>();
        Assert.Equal("Hírek", cut.Find("h2").TextContent);
        Assert.Empty(cut.FindAll(".news-card img"));
        Assert.Equal("<img src=x onerror=alert(1)>", cut.Find(".news-card h3").TextContent);
    }

    private sealed class RetryArticles : IArticleService
    {
        private int _attempts;
        public Task<IReadOnlyList<ArticleDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            if (_attempts++ == 0)
            {
                throw new HttpRequestException("offline");
            }

            return Task.FromResult<IReadOnlyList<ArticleDto>>([new() { Id = "a", Title = "Recovered", MarkdownContent = "Text" }]);
        }

        public Task<ArticleDto?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
            => throw new NotSupportedException();
    }
}

internal sealed class FakeArticles : IArticleService
{
    public TaskCompletionSource<IReadOnlyList<ArticleDto>> List { get; } = new();
    public TaskCompletionSource<ArticleDto?> Detail { get; } = new();
    public Task<IReadOnlyList<ArticleDto>> GetAllAsync(CancellationToken cancellationToken = default) => List.Task;
    public Task<ArticleDto?> GetByIdAsync(string id, CancellationToken cancellationToken = default) => Detail.Task;
}
