using Microsoft.AspNetCore.Components;

using Heiwase.App.Blazor.Components.Shared;

using Radzen;

namespace Heiwase.App.Blazor.Components.Pages.CoachesSection;

public partial class CoachesSection
{
    [Inject]
    public DialogService DialogService { get; set; } = default!;

    protected Task OpenZoltanDialogAsync() =>
        DialogService.OpenAsync<CoachZoltanDialog>("Stempel Zoltán", options: DialogDefaults.Options("min(720px, 92vw)"));

    protected Task OpenBendeguzDialogAsync() =>
        DialogService.OpenAsync<CoachBendeguzDialog>("Tálas Bendegúz", options: DialogDefaults.Options("min(720px, 92vw)"));

    protected Task OpenBarnabasDialogAsync() =>
        DialogService.OpenAsync<CoachBarnabasDialog>("Benkó Barnabás", options: DialogDefaults.Options("min(720px, 92vw)"));
}
