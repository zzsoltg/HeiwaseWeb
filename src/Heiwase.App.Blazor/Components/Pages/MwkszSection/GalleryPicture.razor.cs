using Microsoft.AspNetCore.Components;

namespace Heiwase.App.Blazor.Components.Pages.MwkszSection;

public partial class GalleryPicture
{
    [Parameter, EditorRequired]
    public MwkszGallerySlider.GalleryImage Image { get; set; } = default!;

    [Parameter]
    public string AltText { get; set; } = string.Empty;
}
