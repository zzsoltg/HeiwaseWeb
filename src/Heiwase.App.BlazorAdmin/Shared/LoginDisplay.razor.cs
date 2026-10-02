using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace Heiwase.App.BlazorAdmin.Shared;

public partial class LoginDisplay
{
    [Inject]
    public NavigationManager Navigation { get; set; } = default!;

    private void BeginLogout()
    {
        Navigation.NavigateToLogout("authentication/logout");
    }
}
