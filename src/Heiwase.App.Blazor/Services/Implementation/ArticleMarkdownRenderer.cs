using System.Globalization;
using System.Text.RegularExpressions;
using AngleSharp.Html.Parser;
using Ganss.Xss;
using Heiwase.App.Blazor.Services.Interface;
using Markdig;

namespace Heiwase.App.Blazor.Services.Implementation;

public sealed partial class ArticleMarkdownRenderer : IArticleMarkdownRenderer
{
    private static readonly MarkdownPipeline Pipeline = new MarkdownPipelineBuilder()
        .UseAdvancedExtensions()
        .DisableHtml()
        .Build();

    public string Render(string markdown)
    {
        var sanitizer = new HtmlSanitizer();
        sanitizer.AllowedTags.Clear();
        sanitizer.AllowedTags.UnionWith(["p", "h1", "h2", "h3", "h4", "h5", "h6", "strong", "em", "del", "blockquote", "ul", "ol", "li", "a", "img", "pre", "code", "table", "thead", "tbody", "tr", "th", "td", "hr", "br", "sup", "section", "div", "input"]);
        sanitizer.AllowedAttributes.Clear();
        sanitizer.AllowedAttributes.UnionWith(["href", "src", "alt", "title", "id", "type", "disabled", "checked"]);
        sanitizer.AllowedSchemes.Clear();
        sanitizer.AllowedSchemes.UnionWith(["http", "https", "mailto"]);

        var document = new HtmlParser().ParseDocument(sanitizer.Sanitize(Markdown.ToHtml(markdown, Pipeline)));
        var body = document.Body!;

        foreach (var image in body.QuerySelectorAll("img"))
        {
            if (!IsAllowedImage(image.GetAttribute("src")))
            {
                image.Replace(document.CreateTextNode(image.GetAttribute("alt") ?? string.Empty));
            }
        }

        foreach (var input in body.QuerySelectorAll("input"))
        {
            if (!string.Equals(input.GetAttribute("type"), "checkbox", StringComparison.OrdinalIgnoreCase))
            {
                input.Remove();
                continue;
            }

            input.SetAttribute("disabled", string.Empty);
        }

        foreach (var element in body.QuerySelectorAll("[id]"))
        {
            element.SetAttribute("id", "article-" + element.GetAttribute("id"));
        }

        foreach (var link in body.QuerySelectorAll("a[href]"))
        {
            var href = link.GetAttribute("href")!;
            if (href.StartsWith('#'))
            {
                link.SetAttribute("href", "#article-" + href[1..]);
            }
            else if (!IsAllowedLink(href))
            {
                link.RemoveAttribute("href");
            }
        }

        foreach (var heading in body.QuerySelectorAll("h1, h2, h3, h4, h5"))
        {
            var replacement = document.CreateElement($"h{heading.LocalName[1] - '0' + 1}");
            foreach (var attribute in heading.Attributes)
            {
                replacement.SetAttribute(attribute.Name, attribute.Value);
            }

            replacement.InnerHtml = heading.InnerHtml;
            heading.Replace(replacement);
        }

        foreach (var table in body.QuerySelectorAll("table"))
        {
            var wrapper = document.CreateElement("div");
            wrapper.ClassName = "article-table";
            table.Replace(wrapper);
            wrapper.AppendChild(table);
        }

        return body.InnerHtml;
    }

    public string CreateExcerpt(string markdown, int maxTextElements = 160)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maxTextElements);
        var document = new HtmlParser().ParseDocument(Markdown.ToHtml(markdown, Pipeline));
        foreach (var image in document.QuerySelectorAll("img"))
        {
            image.Remove();
        }

        var text = document.QuerySelectorAll("p")
            .Select(p => Whitespace().Replace(p.TextContent, " ").Trim())
            .FirstOrDefault(p => p.Length > 0) ?? string.Empty;
        var offsets = StringInfo.ParseCombiningCharacters(text);
        if (offsets.Length <= maxTextElements)
        {
            return text;
        }

        var excerpt = text[..offsets[maxTextElements]];
        var lastSpace = excerpt.LastIndexOf(' ');
        if (lastSpace > 0)
        {
            excerpt = excerpt[..lastSpace];
        }

        return excerpt.TrimEnd() + "…";
    }

    [GeneratedRegex(@"\s+")]
    private static partial Regex Whitespace();

    private static bool IsAllowedImage(string? value)
    {
        if (string.IsNullOrWhiteSpace(value)
            || !Uri.TryCreate(value, UriKind.Absolute, out var uri)
            || uri.Scheme != Uri.UriSchemeHttps
            || !string.Equals(uri.IdnHost, "heiwasemedia.blob.core.windows.net", StringComparison.OrdinalIgnoreCase)
            || !uri.IsDefaultPort
            || !string.IsNullOrEmpty(uri.UserInfo))
        {
            return false;
        }

        var path = Uri.UnescapeDataString(uri.AbsolutePath);
        return path.StartsWith("/public-media/", StringComparison.Ordinal)
            && !path.Contains('\\')
            && !path.Split('/').Any(segment => segment is "." or "..");
    }

    private static bool IsAllowedLink(string href)
    {
        if (href.StartsWith('/') && !href.StartsWith("//", StringComparison.Ordinal) && !href.Contains('\\'))
        {
            return true;
        }

        return Uri.TryCreate(href, UriKind.Absolute, out var uri)
            && uri.Scheme is "http" or "https" or "mailto";
    }
}
