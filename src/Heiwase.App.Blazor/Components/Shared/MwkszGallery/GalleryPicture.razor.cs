using Heiwase.App.Blazor.Components.Shared.Dialogs.PictureShowDialog;
using Heiwase.App.Blazor.Helpers;
using Heiwase.App.Blazor.ViewModels;

using Microsoft.AspNetCore.Components;

using Radzen;

namespace Heiwase.App.Blazor.Components.Shared.MwkszGallery;

public partial class GalleryPicture
{
    [Inject]
    public DialogService DialogService { get; set; } = default!;
    [Parameter, EditorRequired]
    public GalleryImageModel Image { get; set; } = default!;
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
