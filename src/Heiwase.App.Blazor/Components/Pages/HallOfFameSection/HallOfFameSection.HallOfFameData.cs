using System.Text.Json.Serialization;

namespace Heiwase.App.Blazor.Components.Pages.HallOfFameSection;

public partial class HallOfFameSection
{
    public class HallOfFameData
    {
        private static readonly List<Member> members = [];

        [JsonPropertyName("competitors")]
        public List<Member> Competitors { get; set; } = members;

        [JsonPropertyName("senpais")]
        public List<Member> Senpais { get; set; } = members;
    }
}
