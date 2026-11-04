namespace Heiwase.App.Blazor.Configuration;

/// <summary>Public Cloudflare Turnstile site key rendered in the browser. The secret key lives only in the API.</summary>
public sealed record TurnstileSettings(string SiteKey);
