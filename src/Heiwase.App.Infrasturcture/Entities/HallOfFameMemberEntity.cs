using System.Text.Json.Serialization;

namespace Heiwase.App.Infrastructure.Entities;

public sealed class HallOfFameMemberEntity : BaseEntity
{
    public const string CompetitorType = "competitor";
    public const string SenpaiType = "senpai";

    /// <summary>Localized texts keyed by language code (for example <c>hu</c>, <c>en</c>).</summary>
    [JsonPropertyName("translations")]
    public Dictionary<string, HallOfFameMemberTranslationEntity> Translations { get; set; } = [ ];

    [JsonPropertyName("imagePath")]
    public string ImagePath { get; set; } = string.Empty;

    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("order")]
    public int Order { get; set; }
}

public sealed class HallOfFameMemberTranslationEntity
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public List<string> Description { get; set; } = [ ];
}