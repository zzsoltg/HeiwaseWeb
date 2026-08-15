using Microsoft.AspNetCore.Components;

using Heiwase.App.Blazor.Components.Shared;

using Radzen;

namespace Heiwase.App.Blazor.Components.Pages.AboutSection;

public partial class AboutSection
{
    [Inject]
    public DialogService DialogService { get; set; } = default!;

    private Task OpenClubHistoryDialogAsync() =>
        DialogService.OpenAsync<ClubHistoryDialog>("1993 óta HEIWASE");

    private Task OpenStudentCountDialogAsync() =>
        DialogService.OpenAsync<StudentCountDialog>("100+ tanítvány");

    private Task OpenLocationDialogAsync() =>
        DialogService.OpenAsync<LocationDialog>("0 km-re Szeged szívétől");
}
