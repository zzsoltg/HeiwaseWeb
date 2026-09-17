using Heiwase.App.Blazor.Components.Shared;
using Heiwase.App.Blazor.Components.Shared.DojoVideoDialog;

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

using Radzen;

namespace Heiwase.App.Blazor.Components.Pages.DojoSection;

public partial class DojoSection
{
    [Inject]
    public IStringLocalizer<DojoSectionResource> L { get; set; } = default!;
    [Inject]
    public DialogService DialogService { get; set; } = default!;

    protected Task OpenVideoDialogAsync( ) =>
        DialogService.OpenAsync<DojoVideoDialog>(L["DojoVideo"], options: DialogDefaults.Options("min(720px, 92vw)", noContentPadding: true));
}
