using System.Net;
using System.Text;
using Heiwase.App.Blazor.Services.Implementation;
using Heiwase.App.Tests.Blazor.Support;

namespace Heiwase.App.Tests.Blazor;

public sealed class ArticleServiceTests
{
    [Fact]
    public async Task FetchesPublishedArticlesFromApi()
    {
        var handler = StubHttpHandler.Json(new[] { new { id = "a", title = "News", markdownContent = "Text", isPublished = true } });
        var service = new ArticleService(handler.CreateClient());

        var articles = await service.GetAllAsync();

        Assert.Equal("News", Assert.Single(articles).Title);
        Assert.Equal("http://localhost/api/articles", Assert.Single(handler.Requests).Uri!.ToString());
    }

    [Fact]
    public async Task ReturnsNullOnlyForMissingArticleAndEscapesId()
    {
        var handler = StubHttpHandler.Status(HttpStatusCode.NotFound);
        var service = new ArticleService(handler.CreateClient());

        Assert.Null(await service.GetByIdAsync("a b"));
        Assert.Equal("/api/articles/a%20b", Assert.Single(handler.Requests).Uri!.AbsolutePath);
    }

    [Fact]
    public async Task ReportsInvalidAndFailedResponses()
    {
        var invalid = new StubHttpHandler(_ => new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("null", Encoding.UTF8, "application/json")
        });
        await Assert.ThrowsAsync<InvalidDataException>(() => new ArticleService(invalid.CreateClient()).GetAllAsync());
        await Assert.ThrowsAsync<HttpRequestException>(() => new ArticleService(StubHttpHandler.Status(HttpStatusCode.InternalServerError).CreateClient()).GetByIdAsync("a"));
    }

    [Theory]
    [InlineData("[null]")]
    [InlineData("[{\"id\":null,\"title\":\"Title\",\"markdownContent\":\"Text\"}]")]
    [InlineData("[{\"id\":\"a\",\"title\":\"Title\",\"markdownContent\":null}]")]
    public async Task RejectsMalformedArticleRecords(string json)
    {
        var handler = new StubHttpHandler(_ => new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        });
        await Assert.ThrowsAsync<InvalidDataException>(() => new ArticleService(handler.CreateClient()).GetAllAsync());
    }
}
