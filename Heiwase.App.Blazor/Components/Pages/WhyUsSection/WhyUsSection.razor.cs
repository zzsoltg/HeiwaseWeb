using Microsoft.AspNetCore.Components;

using Heiwase.App.Blazor.Components.Shared;

using Radzen;

namespace Heiwase.App.Blazor.Components.Pages.WhyUsSection;

public partial class WhyUsSection
{
    [Inject]
    public DialogService DialogService { get; set; } = default!;

    private Task OpenWadoRyuDialogAsync() =>
        DialogService.OpenAsync<WadoRyuDialog>("Wado-ryu", options: DialogDefaults.Options());

    private Task OpenSportkarateDialogAsync() =>
        DialogService.OpenAsync<SportkarateDialog>("Sportkarate", options: DialogDefaults.Options());

    private Task OpenWomensSelfDefenseDialogAsync() =>
        DialogService.OpenAsync<WomensSelfDefenseDialog>("Női önvédelem", options: DialogDefaults.Options());

    private Task OpenAthleticsDialogAsync() =>
        DialogService.OpenAsync<AthleticsDialog>("Atlétika", options: DialogDefaults.Options());
}
