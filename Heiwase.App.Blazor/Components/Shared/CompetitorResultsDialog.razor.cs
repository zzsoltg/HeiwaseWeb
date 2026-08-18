using Heiwase.App.Blazor.Components.Pages.HallOfFameSection;

using Microsoft.AspNetCore.Components;

namespace Heiwase.App.Blazor.Components.Shared;

public partial class CompetitorResultsDialog
{
    [Parameter]
    public HallOfFameSection.Member? Member { get; set; }
}
