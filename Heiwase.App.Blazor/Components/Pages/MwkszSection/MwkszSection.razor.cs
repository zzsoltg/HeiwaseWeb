using Microsoft.AspNetCore.Components;

using Heiwase.App.Blazor.Components.Shared;

using Radzen;
using Microsoft.Extensions.Localization;

namespace Heiwase.App.Blazor.Components.Pages.MwkszSection;

public partial class MwkszSection
{
    [Inject]
    public IStringLocalizer<MwkszSectionResource> L { get; set; } = default!;
    [Inject]
    public DialogService DialogService { get; set; } = default!;

    protected Task OpenBeltExamDialogAsync() =>
        DialogService.OpenAsync<BeltExamDialog>(L["BeltExams"], options: DialogDefaults.Options());

    protected Task OpenTrainingCampDialogAsync() =>
        DialogService.OpenAsync<TrainingCampDialog>(L["TrainingCamps"], options: DialogDefaults.Options());

    protected Task OpenCompetitionsDialogAsync() =>
        DialogService.OpenAsync<CompetitionsDialog>(L["Competitions"], options: DialogDefaults.Options());

    protected Task OpenSeminarsDialogAsync() =>
        DialogService.OpenAsync<SeminarsDialog>(L["Seminars"], options: DialogDefaults.Options());
}
