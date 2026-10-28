using System.Globalization;

using Bunit;

using Microsoft.Extensions.DependencyInjection;

using Radzen;

namespace Heiwase.App.Tests.Blazor.Support;

/// <summary>
/// Shared bUnit context for the site's components: real localization (the production resource files),
/// real Radzen services, loose JS interop and a fixed UI culture so the hu-HU / en-US variants are testable.
/// </summary>
public abstract class SiteTestContext : BunitContext
{
    public const string Hungarian = "hu-HU";
    public const string English = "en-US";

    private readonly CultureInfo _originalCulture = CultureInfo.CurrentCulture;
    private readonly CultureInfo _originalUiCulture = CultureInfo.CurrentUICulture;

    protected SiteTestContext(string culture = English)
    {
        UseCulture(culture);

        JSInterop.Mode = JSRuntimeMode.Loose;
        Services.AddLocalization(options => options.ResourcesPath = "Resources");
        Services.AddRadzenComponents();
    }

    protected static void UseCulture(string culture)
    {
        var info = CultureInfo.GetCultureInfo(culture);
        CultureInfo.CurrentCulture = info;
        CultureInfo.CurrentUICulture = info;
    }

    /// <summary>Records every dialog the components ask Radzen to open and closes it so the awaiting handler completes.</summary>
    protected DialogRecorder RecordDialogs()
        => new(Services.GetRequiredService<DialogService>());

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            CultureInfo.CurrentCulture = _originalCulture;
            CultureInfo.CurrentUICulture = _originalUiCulture;
        }

        base.Dispose(disposing);
    }
}

public sealed record OpenedDialog(string Title, Type ComponentType, IReadOnlyDictionary<string, object?>? Parameters, DialogOptions Options);

public sealed class DialogRecorder
{
    private readonly DialogService _service;

    public DialogRecorder(DialogService service)
    {
        _service = service;
        _service.OnOpen += OnOpen;
    }

    public List<OpenedDialog> Opened { get; } = [];

    public OpenedDialog OnlyDialog => Assert.Single(Opened);

    private void OnOpen(string? title, Type type, Dictionary<string, object?>? parameters, DialogOptions options)
    {
        Opened.Add(new OpenedDialog(title ?? string.Empty, type, parameters, options));
        _service.Close();
    }
}
