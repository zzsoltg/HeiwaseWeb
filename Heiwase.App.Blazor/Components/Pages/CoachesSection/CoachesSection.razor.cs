using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

using Radzen;

using Heiwase.App.Blazor.Components.Shared;
using Heiwase.App.Blazor.Components.Shared.CoachZoltanDialog;
using Heiwase.App.Blazor.Components.Shared.CoachBendeguzDialog;
using Heiwase.App.Blazor.Components.Shared.CoachBarnabasDialog;

namespace Heiwase.App.Blazor.Components.Pages.CoachesSection;

public partial class CoachesSection
{
    [Inject]
    public IStringLocalizer<CoachesSectionResource> L { get; set; } = default!;
    [Inject]
    public DialogService DialogService { get; set; } = default!;

    protected Task OpenZoltanDialogAsync() =>
        DialogService.OpenAsync<CoachZoltanDialog>(L["Zoli"], options: DialogDefaults.Options("min(720px, 92vw)"));

    protected Task OpenBendeguzDialogAsync() =>
        DialogService.OpenAsync<CoachBendeguzDialog>(L["Bendi"], options: DialogDefaults.Options("min(720px, 92vw)"));

    protected Task OpenBarnabasDialogAsync() =>
        DialogService.OpenAsync<CoachBarnabasDialog>(L["Barni"], options: DialogDefaults.Options("min(720px, 92vw)"));
}
