namespace Heiwase.App.Api.Services;

public interface ITurnstileValidator
{
    /// <summary>
    /// Verifies a Cloudflare Turnstile token. Returns <c>false</c> when the token is missing, invalid, expired or already used.
    /// </summary>
    Task<bool> ValidateAsync(string? token, CancellationToken cancellationToken = default);
}
