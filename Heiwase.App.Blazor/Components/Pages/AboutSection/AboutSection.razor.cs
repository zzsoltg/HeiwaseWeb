using Microsoft.AspNetCore.Components;

using Heiwase.App.Blazor.Components.Shared;

using Radzen;
using Microsoft.Extensions.Localization;

namespace Heiwase.App.Blazor.Components.Pages.AboutSection;

public partial class AboutSection
{
    [Inject]
    public IStringLocalizer<AboutSectionResource> L { get; set; } = default!;
    [Inject]
    public DialogService DialogService { get; set; } = default!;

    protected Task OpenClubHistoryDialogAsync() =>
        DialogService.OpenAsync<ClubHistoryDialog>(L["History"], options: DialogDefaults.Options());

    protected Task OpenStudentCountDialogAsync() =>
        DialogService.OpenAsync<StudentCountDialog>(L["Mission"], options: DialogDefaults.Options());

    protected Task OpenMedalDialogAsync() =>
        DialogService.OpenAsync<MedalDialog>(L["Achievements"], options: DialogDefaults.Options());
}
