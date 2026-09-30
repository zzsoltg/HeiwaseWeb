using Heiwase.App.Blazor.Components.Pages.HallOfFameSection;
using Heiwase.App.Shared.Models;

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace Heiwase.App.Blazor.Components.Shared.Dialogs.CompetitorResultsDialog;

public partial class CompetitorResultsDialog
{
    [Inject]
    public IStringLocalizer<CompetitorResultsDialogResource> L { get; set; } = default!;
    [Parameter]
    public HallOfFameMemberDto? Member { get; set; }
}
