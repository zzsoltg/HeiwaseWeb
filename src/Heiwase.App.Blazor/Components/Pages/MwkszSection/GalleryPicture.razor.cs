using Heiwase.App.Blazor.Components.Shared;
using Heiwase.App.Blazor.Components.Shared.Dialogs.PictureShowDialog;

using Microsoft.AspNetCore.Components;

using Radzen;

namespace Heiwase.App.Blazor.Components.Pages.MwkszSection;

public partial class GalleryPicture
{
    [Inject]
    public DialogService DialogService { get; set; } = default!;
    [Parameter, EditorRequired]
    public MwkszGallerySlider.GalleryImage Image { get; set; } = default!;
    [Parameter]
    public string AltText { get; set; } = string.Empty;

    public async Task OpenImageDialog( )
    {
        await DialogService.OpenAsync<PictureShowDialog>
        (AltText,
            new Dictionary<string, object?>( )
            {
                { "ImagePath", Image.Source },
                { "AltText", AltText }
            },
            options: DialogDefaults.Options("auto", noContentPadding: true)
        );
    }
}
