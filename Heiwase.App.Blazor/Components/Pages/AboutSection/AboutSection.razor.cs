using Microsoft.AspNetCore.Components;

using Heiwase.App.Blazor.Components.Shared;

using Radzen;

namespace Heiwase.App.Blazor.Components.Pages.AboutSection;

public partial class AboutSection
{
    [Inject]
    public DialogService DialogService { get; set; } = default!;

    private Task OpenClubHistoryDialogAsync() =>
        DialogService.OpenAsync<ClubHistoryDialog>("Történetünk", options: DialogDefaults.Options());

    private Task OpenStudentCountDialogAsync() =>
        DialogService.OpenAsync<StudentCountDialog>("Küldetésünk", options: DialogDefaults.Options());

    private Task OpenMedalDialogAsync() =>
        DialogService.OpenAsync<MedalDialog>("Eredményeink", options: DialogDefaults.Options());
}
