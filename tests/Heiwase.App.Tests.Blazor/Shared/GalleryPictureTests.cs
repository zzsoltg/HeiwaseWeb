using Bunit;

using Heiwase.App.Blazor.Components.Shared.MwkszGallery;
using Heiwase.App.Blazor.ViewModels;
using Heiwase.App.Tests.Blazor.Support;

namespace Heiwase.App.Tests.Blazor.Shared;

public sealed class GalleryPictureTests : SiteTestContext
{
    private static readonly GalleryImageModel Sample = new("https://media.example/img/mwkszgrid/csoport.jpg", "g-item-1");

    [Fact]
    public void Derives_Modern_Format_Sources_From_The_Canonical_Jpg()
    {
        IRenderedComponent<GalleryPicture> cut = Render<GalleryPicture>(p => p
            .Add(c => c.Image, Sample)
            .Add(c => c.AltText, "Group photo"));

        Assert.Equal("https://media.example/img/mwkszgrid/csoport.avif", cut.Find("source[type='image/avif']").GetAttribute("srcset"));
        Assert.Equal("https://media.example/img/mwkszgrid/csoport.webp", cut.Find("source[type='image/webp']").GetAttribute("srcset"));
        Assert.Equal("https://media.example/img/mwkszgrid/csoport.jpg", cut.Find("img").GetAttribute("src"));
        Assert.Equal("Group photo", cut.Find("img").GetAttribute("alt"));
        Assert.Contains("g-item-1", cut.Find("picture").ClassList);
    }

    [Fact]
    public void Click_Opens_The_Image_In_A_Borderless_Picture_Dialog()
    {
        DialogRecorder dialogs = RecordDialogs();
        IRenderedComponent<GalleryPicture> cut = Render<GalleryPicture>(p => p
            .Add(c => c.Image, Sample)
            .Add(c => c.AltText, "Group photo"));

        cut.Find("picture").Click();

        OpenedDialog dialog = dialogs.OnlyDialog;
        Assert.Equal("PictureShowDialog", dialog.ComponentType.Name);
        Assert.Equal("Group photo", dialog.Title);
        Assert.Equal(Sample.Source, dialog.Parameters!["ImagePath"]);
        Assert.Equal("Group photo", dialog.Parameters["AltText"]);
        Assert.Equal("auto", dialog.Options.Width);
        Assert.Contains("app-dialog-content-flush", dialog.Options.ContentCssClass, StringComparison.Ordinal);
    }
}
