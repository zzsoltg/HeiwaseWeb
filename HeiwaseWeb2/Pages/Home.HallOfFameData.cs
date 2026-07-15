using System.Text.Json.Serialization;
namespace HeiwaseWeb2.Pages;

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
