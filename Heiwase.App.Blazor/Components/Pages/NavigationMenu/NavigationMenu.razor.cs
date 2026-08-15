namespace Heiwase.App.Blazor.Components.Pages.NavigationMenu;

public partial class NavigationMenu
{
    private bool _isMenuOpen = false;

    private void ToggleMenu() => _isMenuOpen = !_isMenuOpen;

    private void CloseMenu() => _isMenuOpen = false;
}
