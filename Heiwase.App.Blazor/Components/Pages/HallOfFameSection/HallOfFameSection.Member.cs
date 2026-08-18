namespace Heiwase.App.Blazor.Components.Pages.HallOfFameSection;

public partial class HallOfFameSection
{
    public class Member
    {
        public string Name { get; set; } = "";
        public string Title { get; set; } = "";
        public List<string> Description { get; set; } = [];
        public string ImagePath { get; set; } = "";
    }
}
