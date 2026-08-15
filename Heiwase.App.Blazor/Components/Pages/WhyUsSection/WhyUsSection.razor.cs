using Microsoft.AspNetCore.Components;

using Heiwase.App.Blazor.Components.Shared;

using Radzen;

namespace Heiwase.App.Blazor.Components.Pages.WhyUsSection;

public partial class WhyUsSection
{
    [Inject]
    public DialogService DialogService { get; set; } = default!;

    private Task OpenWadoRyuDialogAsync() =>
        DialogService.OpenAsync<WadoRyuDialog>("Wado-ryu");

    private Task OpenSportkarateDialogAsync() =>
        DialogService.OpenAsync<SportkarateDialog>("Sportkarate");

    private Task OpenWomensSelfDefenseDialogAsync() =>
        DialogService.OpenAsync<WomensSelfDefenseDialog>("Női önvédelem");

    private Task OpenAthleticsDialogAsync() =>
        DialogService.OpenAsync<AthleticsDialog>("Atlétika");
}
