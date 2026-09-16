namespace Heiwase.App.Blazor.Components.Pages.HallOfFameSection;

public partial class HallOfFameSection
{
    public class Member
    {
        public string Name { get; set; } = String.Empty;
        public string Title { get; set; } = String.Empty;
        public List<string> Description { get; set; } = [];
        public string ImagePath { get; set; } = String.Empty;

        public string AvifPath => Path.ChangeExtension(ImagePath, ".avif");
        public string WebpPath => Path.ChangeExtension(ImagePath, ".webp");
        public string JpgPath => Path.ChangeExtension(ImagePath, ".jpg");
        public string PngPath => Path.ChangeExtension(ImagePath, ".png");
    }
}
