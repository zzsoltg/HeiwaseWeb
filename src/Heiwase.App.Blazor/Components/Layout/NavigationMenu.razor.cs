using System.Globalization;

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;

namespace Heiwase.App.Blazor.Components.Layout;

public partial class NavigationMenu
{
    [Inject]
    public IStringLocalizer<NavigationMenuResource> L { get; set; } = default!;
    private bool _isMenuOpen;

    private void ToggleMenu()
        => _isMenuOpen = !_isMenuOpen;

    private void CloseMenu()
        => _isMenuOpen = false;
}
