using System.Text.Json.Serialization;

namespace Heiwase.App.Infrasturcture.Entities;

public class HallOfFameMember
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("rank")]
    public string Rank { get; set; } = string.Empty;

    [JsonPropertyName("achievements")]
    public List<string> Achievements { get; set; } = [ ];

    [JsonPropertyName("imageUrl")]
    public string ImageUrl { get; set; } = string.Empty;

    /// <summary>
    /// Cosmos DB partition key: "competitor" or "senpai"
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;
}
