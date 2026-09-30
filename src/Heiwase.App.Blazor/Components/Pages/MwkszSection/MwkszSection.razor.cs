using Heiwase.App.Blazor.Components.Shared.Dialogs.BeltExamDialog;
using Heiwase.App.Blazor.Components.Shared.Dialogs.CompetitionsDialog;
using Heiwase.App.Blazor.Components.Shared.Dialogs.SeminarsDialog;
using Heiwase.App.Blazor.Components.Shared.Dialogs.TrainingCampDialog;
using Heiwase.App.Blazor.Helpers;

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

using Radzen;

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
