using Heiwase.App.Blazor.Components.Shared.Dialogs.DojoVideoDialog;
using Heiwase.App.Blazor.Components.Shared.Dialogs.PictureShowDialog;
using Heiwase.App.Blazor.Helpers;

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

using Radzen;

namespace Heiwase.App.Blazor.Components.Sections.Home.DojoSection;

public partial class DojoSection
{
    [Inject]
    public IStringLocalizer<DojoSectionResource> L { get; set; } = default!;
    [Inject]
    public DialogService DialogService { get; set; } = default!;

    protected const string MediaBaseUrl = "https://heiwasemedia.blob.core.windows.net/public-media/img/dojogrid";

    protected Task OpenVideoDialogAsync( ) =>
        DialogService.OpenAsync<DojoVideoDialog>(L["DojoVideo"], options: DialogDefaults.Options("min(720px, 92vw)", noContentPadding: true));

    protected Task OpenImageDialog(string imagePath) =>
        DialogService.OpenAsync<PictureShowDialog>
        (
            L["DojoVideo"].Value,
            new Dictionary<string, object?>
            {
                { "ImagePath", imagePath },
                { "AltText", L["DojoVideo"].Value }
            },
            options: DialogDefaults.Options("auto", noContentPadding: true)
        );
}
