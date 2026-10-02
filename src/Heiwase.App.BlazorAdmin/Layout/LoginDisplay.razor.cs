using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace Heiwase.App.BlazorAdmin.Layout;

public partial class LoginDisplay
{
    [Inject]
    public NavigationManager Navigation { get; set; } = default!;

    private void BeginLogout()
    {
        Navigation.NavigateToLogout("authentication/logout");
    }
}
