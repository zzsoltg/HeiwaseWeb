using Radzen;

namespace Heiwase.App.Blazor.Components.Shared;

/// <summary>
/// Provides a single, consistent <see cref="DialogOptions"/> configuration so every
/// dialog on the site shares the same frame (size, overlay behaviour, styling hooks),
/// regardless of which section opens it.
/// </summary>
public static class DialogDefaults
{
    /// <summary>
    /// Builds the shared dialog frame options. Pass <paramref name="width"/> to override
    /// the default width for dialogs that need more or less room (e.g. CV-style coach dialogs).
    /// </summary>
    public static DialogOptions Options(string width = "min(680px, 92vw)") => new()
    {
        Width = width,
        CssClass = "app-dialog",
        ContentCssClass = "app-dialog-content",
        ShowTitle = true,
        ShowClose = true,
        CloseDialogOnEsc = true,
        CloseDialogOnOverlayClick = true,
        Draggable = false,
        Resizable = false,
    };
}
