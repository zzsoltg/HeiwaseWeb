using Bunit;
using Heiwase.App.Blazor.Components.Pages;
using Heiwase.App.Blazor.Services.Implementation;
using Heiwase.App.Blazor.Services.Interface;
using Heiwase.App.Shared.Models;
using Heiwase.App.Tests.Blazor.Support;
using Microsoft.Extensions.DependencyInjection;

namespace Heiwase.App.Tests.Blazor.Sections;

public sealed class ArticleDetailTests : SiteTestContext
{
    [Fact]
    public void LoadsAndRendersArticleMarkdown()
    {
        var source = new FakeArticles();
        Services.AddSingleton<IArticleService>(source);
        Services.AddSingleton<IArticleMarkdownRenderer, ArticleMarkdownRenderer>();
        var cut = Render<ArticleDetail>(parameters => parameters.Add(p => p.Id, "first"));
        Assert.NotEmpty(cut.FindAll("[role=status]"));

        source.Detail.SetResult(new ArticleDto { Id = "first", Title = "Article title", MarkdownContent = "Hello **world**" });
        cut.WaitForAssertion(() => Assert.Equal("Article title", cut.Find("h1").TextContent));
        Assert.Equal("world", cut.Find(".article-content strong").TextContent);
        Assert.Equal("/#news", cut.Find(".article-back").GetAttribute("href"));
    }

    [Fact]
    public void ShowsMissingArticle()
    {
        var source = new FakeArticles();
        source.Detail.SetResult(null);
        Services.AddSingleton<IArticleService>(source);
        Services.AddSingleton<IArticleMarkdownRenderer, ArticleMarkdownRenderer>();
        var cut = Render<ArticleDetail>(parameters => parameters.Add(p => p.Id, "missing"));
        cut.WaitForAssertion(() => Assert.NotEmpty(cut.FindAll(".article-not-found")));
    }

    [Fact]
    public void ReportsNetworkFailureInsteadOfShowingNotFound()
    {
        var source = new FakeArticles();
        source.Detail.SetException(new HttpRequestException("offline"));
        Services.AddSingleton<IArticleService>(source);
        Services.AddSingleton<IArticleMarkdownRenderer, ArticleMarkdownRenderer>();
        var cut = Render<ArticleDetail>(parameters => parameters.Add(p => p.Id, "first"));
        cut.WaitForAssertion(() => Assert.NotEmpty(cut.FindAll("[role=alert]")));
        Assert.Empty(cut.FindAll(".article-not-found"));
    }

    [Fact]
    public void LateResponseCannotReplaceNewArticle()
    {
        var source = new SwitchingArticles();
        Services.AddSingleton<IArticleService>(source);
        Services.AddSingleton<IArticleMarkdownRenderer, ArticleMarkdownRenderer>();
        var cut = Render<ArticleDetail>(parameters => parameters.Add(p => p.Id, "old"));
        cut.Render(parameters => parameters.Add(p => p.Id, "new"));

        source.New.SetResult(new ArticleDto { Id = "new", Title = "New", MarkdownContent = "New text" });
        cut.WaitForAssertion(() => Assert.Equal("New", cut.Find("h1").TextContent));
        source.Old.SetResult(new ArticleDto { Id = "old", Title = "Old", MarkdownContent = "Old text" });
        cut.WaitForAssertion(() => Assert.Equal("New", cut.Find("h1").TextContent));
    }

    [Fact]
    public void RendererFailureShowsErrorWithoutUnsafeContent()
    {
        var source = new FakeArticles();
        source.Detail.SetResult(new ArticleDto { Id = "a", Title = "Title", MarkdownContent = "<script>alert(1)</script>" });
        Services.AddSingleton<IArticleService>(source);
        Services.AddSingleton<IArticleMarkdownRenderer>(new FailingRenderer());
        var cut = Render<ArticleDetail>(parameters => parameters.Add(p => p.Id, "a"));
        cut.WaitForAssertion(() => Assert.NotEmpty(cut.FindAll("[role=alert]")));
        Assert.Empty(cut.FindAll(".article-content, script"));
    }

    private sealed class FailingRenderer : IArticleMarkdownRenderer
    {
        public string Render(string markdown) => throw new InvalidOperationException("Sanitization failed");
        public string CreateExcerpt(string markdown, int maxTextElements = 160) => throw new NotSupportedException();
    }

    private sealed class SwitchingArticles : IArticleService
    {
        public TaskCompletionSource<ArticleDto?> Old { get; } = new();
        public TaskCompletionSource<ArticleDto?> New { get; } = new();
        public Task<IReadOnlyList<ArticleDto>> GetAllAsync(CancellationToken cancellationToken = default)
            => throw new NotSupportedException();
        public Task<ArticleDto?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
            => id == "old" ? Old.Task : New.Task;
    }
}
