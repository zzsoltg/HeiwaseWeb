using Microsoft.AspNetCore.Components;

namespace Heiwase.App.BlazorAdmin.Pages;

public partial class Authentication
{
    [Parameter]
    public string? Action { get; set; }
}
