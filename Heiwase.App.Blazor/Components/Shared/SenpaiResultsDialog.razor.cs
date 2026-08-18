using Heiwase.App.Blazor.Components.Pages.HallOfFameSection;

using Microsoft.AspNetCore.Components;

namespace Heiwase.App.Blazor.Components.Shared;

public partial class SenpaiResultsDialog
{
    [Parameter]
    public HallOfFameSection.Member? Member { get; set; }
    private string RoleTitle =>
    Member?.Title?.EndsWith("dan", StringComparison.OrdinalIgnoreCase) == true
        ? "Sensei"
        : "Senpai";
}