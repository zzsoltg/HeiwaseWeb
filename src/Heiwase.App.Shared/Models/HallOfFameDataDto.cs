using System.Text.Json.Serialization;

namespace Heiwase.App.Shared.Models;

public class HallOfFameDataDto
{
    private static readonly List<HallOfFameMemberDto> members = [ ];

    [JsonPropertyName("competitors")]
    public List<HallOfFameMemberDto> Competitors { get; set; } = members;

    [JsonPropertyName("senpais")]
    public List<HallOfFameMemberDto> Senpais { get; set; } = members;
}
