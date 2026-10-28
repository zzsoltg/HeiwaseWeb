using System.Globalization;

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Heiwase.App.Blazor.Components.Layout;

public partial class LanguageSelector
{
    [Inject]
    public IJSRuntime JS { get; set; } = default!;
    [Inject]
    public NavigationManager NavManager { get; set; } = default!;

    private async Task ChangeLanguageAsync()
    {
        var newCulture = CultureInfo.CurrentCulture.Name == "hu-HU" ? "en-US" : "hu-HU";
        await JS.InvokeVoidAsync("localStorage.setItem", "blazorCulture", newCulture);
        NavManager.NavigateTo(NavManager.Uri, forceLoad: true);
    }

    private static string GetCurrentPicturePath()
    {
        return CultureInfo.CurrentCulture.Name == "hu-HU" ? "img/eng-lang-ico.svg" : "img/hun-lang-ico.svg";
    }
}
