using AngleSharp.Html.Parser;
using Heiwase.App.Blazor.Services.Implementation;

namespace Heiwase.App.Tests.Blazor;

public sealed class ArticleMarkdownRendererTests
{
    private readonly ArticleMarkdownRenderer _renderer = new();

    [Fact]
    public void RendersFormattedMarkdownAndDemotesHeadings()
    {
        var document = new HtmlParser().ParseDocument(_renderer.Render("# Heading\n\nA **bold** and *italic* paragraph.\n\n> Quote\n\n| A | B |\n|---|---|\n| 1 | 2 |"));
        Assert.Equal("Heading", document.QuerySelector("h2")?.TextContent);
        Assert.Equal("bold", document.QuerySelector("strong")?.TextContent);
        Assert.Equal("italic", document.QuerySelector("em")?.TextContent);
        Assert.NotNull(document.QuerySelector("blockquote"));
        Assert.NotNull(document.QuerySelector("table"));
    }

    [Fact]
    public void KeepsApprovedImagesAtTheirPositionAndRejectsOtherUrls()
    {
        const string markdown = "First\n\n![One](https://heiwasemedia.blob.core.windows.net/public-media/img/one.jpg)\n\nMiddle\n\n![Bad](https://heiwasemedia.blob.core.windows.net.evil.example/public-media/bad.jpg)\n\n![Two](https://heiwasemedia.blob.core.windows.net/public-media/img/two.jpg)";
        var document = new HtmlParser().ParseDocument(_renderer.Render(markdown));
        var images = document.QuerySelectorAll("img");
        Assert.Equal(2, images.Length);
        Assert.Equal("One", images[0].GetAttribute("alt"));
        Assert.Equal("Two", images[1].GetAttribute("alt"));
        Assert.Contains("Bad", document.Body!.TextContent);
        Assert.True(document.Body.TextContent.IndexOf("Middle", StringComparison.Ordinal) < document.Body.TextContent.IndexOf("Bad", StringComparison.Ordinal));
    }

    [Theory]
    [InlineData("![bad](javascript:alert(1))")]
    [InlineData("![bad](data:image/svg+xml,abc)")]
    [InlineData("![bad](//evil.example/image.png)")]
    [InlineData("![bad](https://evil.example/image.png)")]
    [InlineData("![bad](https://heiwasemedia.blob.core.windows.net/other/image.png)")]
    [InlineData("![bad](https://heiwasemedia.blob.core.windows.net@evil.example/public-media/image.png)")]
    [InlineData("![bad](https://heiwasemedia.blob.core.windows.net:444/public-media/image.png)")]
    [InlineData("![bad](https://heiwasemedia.blob.core.windows.net/public-media/%2e%2e/other/image.png)")]
    [InlineData("<img src=x onerror=alert(1)>")]
    [InlineData("<script>alert(1)</script>")]
    public void NeverRendersUnsafeImagesOrScripts(string markdown)
    {
        var document = new HtmlParser().ParseDocument(_renderer.Render(markdown));
        Assert.Empty(document.QuerySelectorAll("script, img, iframe, svg, [onerror]"));
    }

    [Fact]
    public void ExtractsPlainTextExcerptFromFirstParagraph()
    {
        Assert.Equal("Hello brave world", _renderer.CreateExcerpt("# Title\n\nHello **brave** [world](https://example.com)\n\nNext paragraph."));
        Assert.Equal("", _renderer.CreateExcerpt("# Heading\n\n![Image](https://heiwasemedia.blob.core.windows.net/public-media/a.jpg)"));
        Assert.Equal("Actual text", _renderer.CreateExcerpt("![Ignore](https://evil.example/a.jpg)\n\nActual text"));
    }

    [Fact]
    public void PrefixesFragmentIdentifiersAndKeepsFootnotesUsable()
    {
        var document = new HtmlParser().ParseDocument(_renderer.Render("# Heading\n\nA note.[^1]\n\n[^1]: Note text"));
        var fragment = document.QuerySelector("a[href^='#article-']");
        Assert.NotNull(fragment);
        Assert.NotNull(document.GetElementById(fragment.GetAttribute("href")![1..]));
        Assert.Empty(document.QuerySelectorAll("[name], [style], [onclick]"));
    }
}
