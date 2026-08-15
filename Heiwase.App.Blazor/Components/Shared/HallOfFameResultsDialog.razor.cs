using Microsoft.AspNetCore.Components;

using Heiwase.App.Blazor.Components.Pages.HallOfFameSection;

namespace Heiwase.App.Blazor.Components.Shared;

public partial class HallOfFameResultsDialog
{
	[Parameter]
	public HallOfFameSection.Member? Member { get; set; }

	// Mocked accomplishments shown for every Hall of Fame member until real, per-athlete
	// result history is wired up. Both CompetitorResultsDialog and SenpaiResultsDialog
	// render this same content today; they can diverge later without touching call sites.
	private static readonly string[] MockedAccomplishments =
	[
		"Több szoros, taktikus mérkőzés megnyerése az idei szezon során",
		"Rendszeres részvétel a klub edzőtáboraiban és felkészítő szemináriumain",
		"Aktív tagja a Heiwase versenyzői keretének",
		"Példamutató hozzáállás és fejlődés az elmúlt évek edzésein"
	];
}
