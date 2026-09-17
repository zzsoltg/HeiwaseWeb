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
    /// Pass <paramref name="noContentPadding"/> for dialogs whose content must span edge-to-edge
    /// (e.g. a video player), which suppresses the default content padding without affecting other dialogs.
    /// </summary>
    public static DialogOptions Options(string width = "min(680px, 92vw)", bool noContentPadding = false) => new()
    {
        Width = width,
        CssClass = "app-dialog",
        ContentCssClass = noContentPadding ?
            "app-dialog-content-flush rz-dialog-content-flush rz-dialog-side-content-flush app-dialog-content rz-dialog-content rz-dialog-side-content"
            : "app-dialog-content rz-dialog-content rz-dialog-side-content",
        ShowTitle = true,
        ShowClose = true,
        CloseDialogOnEsc = true,
        CloseDialogOnOverlayClick = true,
        Draggable = false,
        Resizable = false,
    };
}
