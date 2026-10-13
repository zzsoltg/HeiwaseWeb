using System.Text.Json.Serialization;

namespace Heiwase.App.Infrastructure.Entities;

public sealed class Article : BaseEntity
{
    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("markdownContent")]
    public string MarkdownContent { get; set; } = string.Empty;

    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("isPublished")]
    public bool IsPublished { get; set; }
}
