using System.Text.Json.Serialization;

namespace Heiwase.App.Infrastructure.Entities;

public sealed class HallOfFameMemberEntity : BaseEntity
{
    public const string CompetitorType = "competitor";
    public const string SenpaiType = "senpai";

    /// <summary>Localized texts are stored per language code (for example <c>hu</c>, <c>en</c>).</summary>
    [JsonPropertyName("name")]
    public Dictionary<string, string> Name { get; set; } = [ ];

    [JsonPropertyName("title")]
    public Dictionary<string, string> Title { get; set; } = [ ];

    [JsonPropertyName("description")]
    public Dictionary<string, List<string>> Description { get; set; } = [ ];

    [JsonPropertyName("imagePath")]
    public string ImagePath { get; set; } = string.Empty;

    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("order")]
    public int Order { get; set; }
}