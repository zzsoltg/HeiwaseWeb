using Microsoft.AspNetCore.Components;

using Heiwase.App.Blazor.Components.Shared;

using Radzen;
using Microsoft.Extensions.Localization;

namespace Heiwase.App.Blazor.Components.Pages.WhyUsSection;

public partial class WhyUsSection
{
    [Inject]
    public IStringLocalizer<WhyUsSectionResource> L { get; set; } = default!;
    [Inject]
    public DialogService DialogService { get; set; } = default!;

    protected Task OpenWadoRyuDialogAsync() =>
        DialogService.OpenAsync<WadoRyuDialog>(L["WadoRyu"], options: DialogDefaults.Options());

    protected Task OpenSportkarateDialogAsync() =>
        DialogService.OpenAsync<SportkarateDialog>(L["Sportkarate"], options: DialogDefaults.Options());

    protected Task OpenWomensSelfDefenseDialogAsync() =>
        DialogService.OpenAsync<WomensSelfDefenseDialog>(L["SelfDefense"], options: DialogDefaults.Options());

    protected Task OpenAthleticsDialogAsync() =>
        DialogService.OpenAsync<AthleticsDialog>(L["Athletics"], options: DialogDefaults.Options());
}
