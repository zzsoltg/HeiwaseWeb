using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace Heiwase.App.BlazorAdmin.Layout;

public partial class RedirectToLogin
{
    [Inject]
    public NavigationManager Navigation { get; set; } = default!;

    protected override void OnInitialized()
    {
        Navigation.NavigateToLogin("authentication/login");
    }
}
