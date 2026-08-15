using Microsoft.AspNetCore.Components;

using Heiwase.App.Blazor.Components.Shared;

using Radzen;

namespace Heiwase.App.Blazor.Components.Pages.MwkszSection;

public partial class MwkszSection
{
    [Inject]
    public DialogService DialogService { get; set; } = default!;

    private Task OpenBeltExamDialogAsync() =>
        DialogService.OpenAsync<BeltExamDialog>("Övvizsgák");

    private Task OpenTrainingCampDialogAsync() =>
        DialogService.OpenAsync<TrainingCampDialog>("Edzőtáborok");

    private Task OpenCompetitionsDialogAsync() =>
        DialogService.OpenAsync<CompetitionsDialog>("Versenyek");

    private Task OpenSeminarsDialogAsync() =>
        DialogService.OpenAsync<SeminarsDialog>("Szemináriumok");
}
