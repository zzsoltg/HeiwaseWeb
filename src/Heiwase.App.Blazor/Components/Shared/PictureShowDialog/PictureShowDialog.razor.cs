using Microsoft.AspNetCore.Components;

namespace Heiwase.App.Blazor.Components.Shared.PictureShowDialog;

public partial class PictureShowDialog
{
    [Parameter, EditorRequired]
    public string ImagePath { get; set; } = string.Empty;
    [Parameter]
    public string AltText { get; set; } = string.Empty;
}
