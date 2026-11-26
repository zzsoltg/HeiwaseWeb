namespace Heiwase.App.Blazor.Services.Interface;

public interface IArticleMarkdownRenderer
{
    string Render(string markdown);
    string CreateExcerpt(string markdown, int maxTextElements = 160);
}
