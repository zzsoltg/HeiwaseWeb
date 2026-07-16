using System.Text.Json.Serialization;
namespace Heiwase.App.Blazor.Pages.Home;

public partial class Home
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
