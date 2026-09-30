using Heiwase.App.Blazor.Components.Pages.HallOfFameSection;
using Heiwase.App.Shared.Models;

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace Heiwase.App.Blazor.Components.Shared.Dialogs.SenpaiResultsDialog;

public partial class SenpaiResultsDialog
{
    [Inject]
    public IStringLocalizer<SenpaiResultsDialogResource> L { get; set; } = default!;
    [Parameter]
    public HallOfFameMemberDto? Member { get; set; }
    private string RoleTitle =>
    Member?.Title?.EndsWith("dan", StringComparison.OrdinalIgnoreCase) == true
        ? "Sensei"
        : "Senpai";
}