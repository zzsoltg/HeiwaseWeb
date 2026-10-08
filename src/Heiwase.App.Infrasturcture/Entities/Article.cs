using System.Text.Json.Serialization;

namespace Heiwase.App.Infrasturcture.Entities;

public class Article
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("markdownContent")]
    public string MarkdownContent { get; set; } = string.Empty;

    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; set; }
}
