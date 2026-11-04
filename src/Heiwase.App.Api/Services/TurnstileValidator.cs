using System.Text.Json;

using Heiwase.App.Api.Configuration;

using Microsoft.Extensions.Options;

namespace Heiwase.App.Api.Services;

public sealed class TurnstileValidator : ITurnstileValidator
{
    public const string VerifyEndpoint = "https://challenges.cloudflare.com/turnstile/v0/siteverify";

    private readonly HttpClient _http;
    private readonly string _secretKey;

    public TurnstileValidator(HttpClient http, IOptions<TurnstileOptions> options)
    {
        ArgumentNullException.ThrowIfNull(http);
        ArgumentNullException.ThrowIfNull(options);

        _http = http;
        _secretKey = options.Value.SecretKey;
    }

    public async Task<bool> ValidateAsync(string? token, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            return false;
        }

        using FormUrlEncodedContent content = new(new Dictionary<string, string>
        {
            ["secret"] = _secretKey,
            ["response"] = token
        });

        using var response = await _http.PostAsync(new Uri(VerifyEndpoint), content, cancellationToken).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();

        await using var body = await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
        using var document = await JsonDocument.ParseAsync(body, cancellationToken: cancellationToken).ConfigureAwait(false);

        return document.RootElement.TryGetProperty("success", out var success)
            && success.ValueKind == JsonValueKind.True;
    }
}
