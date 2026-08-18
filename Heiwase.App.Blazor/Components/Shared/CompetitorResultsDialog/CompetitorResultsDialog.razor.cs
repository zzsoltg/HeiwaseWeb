using Heiwase.App.Blazor.Components.Pages.HallOfFameSection;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace Heiwase.App.Blazor.Components.Shared.CompetitorResultsDialog;

public partial class CompetitorResultsDialog
{
    [Inject]
    public IStringLocalizer<CompetitorResultsDialogResource> L { get; set; } = default!;
    [Parameter]
    public HallOfFameSection.Member? Member { get; set; }
}
