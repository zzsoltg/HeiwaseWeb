using System.Text.Json.Serialization;

namespace Heiwase.App.Shared.Models;

public class HallOfFameDataDto
{
    [JsonPropertyName("competitors")]
    public List<HallOfFameMemberDto> Competitors { get; set; } = [];

    [JsonPropertyName("senpais")]
    public List<HallOfFameMemberDto> Senpais { get; set; } = [];
}
