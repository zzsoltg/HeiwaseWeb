using System.Text.Json.Serialization;

namespace Heiwase.App.Infrastructure.Entities;

public abstract class BaseEntity
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;
}
