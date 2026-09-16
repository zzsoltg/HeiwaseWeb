using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;

using System.Globalization;

namespace Heiwase.App.Blazor.Components.Layout.NavigationMenu;

public partial class NavigationMenu
{
    [Inject]
    public IStringLocalizer<NavigationMenuResource> L { get; set; } = default!;
    private bool _isMenuOpen = false;

    private void ToggleMenu()
        => _isMenuOpen = !_isMenuOpen;

    private void CloseMenu()
        => _isMenuOpen = false;
}
