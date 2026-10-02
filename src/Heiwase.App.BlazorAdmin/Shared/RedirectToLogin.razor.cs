using Microsoft.AspNetCore.Components;

namespace Heiwase.App.BlazorAdmin.Shared;

public partial class RedirectToLogin
{
    [Inject]
    public NavigationManager Navigation { get; set; } = default!;

    protected override void OnInitialized()
    {
        Navigation.NavigateTo($"authentication/login?returnUrl={Uri.EscapeDataString(Navigation.Uri)}");
    }
}