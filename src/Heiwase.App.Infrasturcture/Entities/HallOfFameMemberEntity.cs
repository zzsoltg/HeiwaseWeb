using System.Text.Json.Serialization;

namespace Heiwase.App.Infrastructure.Entities;

public sealed class HallOfFameMemberEntity : BaseEntity
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public List<string> Description { get; set; } = [ ];

    [JsonPropertyName("imagePath")]
    public string ImagePath { get; set; } = string.Empty;


    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("order")]
    public int Order { get; set; }
}