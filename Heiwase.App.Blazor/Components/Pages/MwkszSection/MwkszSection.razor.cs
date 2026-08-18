using Microsoft.AspNetCore.Components;

using Heiwase.App.Blazor.Components.Shared;

using Radzen;

namespace Heiwase.App.Blazor.Components.Pages.MwkszSection;

public partial class MwkszSection
{
    [Inject]
    public DialogService DialogService { get; set; } = default!;

    private Task OpenBeltExamDialogAsync() =>
        DialogService.OpenAsync<BeltExamDialog>("Övvizsgák", options: DialogDefaults.Options());

    private Task OpenTrainingCampDialogAsync() =>
        DialogService.OpenAsync<TrainingCampDialog>("Edzőtáborok", options: DialogDefaults.Options());

    private Task OpenCompetitionsDialogAsync() =>
        DialogService.OpenAsync<CompetitionsDialog>("Versenyek", options: DialogDefaults.Options());

    private Task OpenSeminarsDialogAsync() =>
        DialogService.OpenAsync<SeminarsDialog>("Szemináriumok", options: DialogDefaults.Options());
}
