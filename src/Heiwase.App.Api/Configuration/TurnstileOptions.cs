using System.ComponentModel.DataAnnotations;

namespace Heiwase.App.Api.Configuration;

public sealed class TurnstileOptions
{
    public const string SectionName = "Turnstile";

    [Required]
    public string SecretKey { get; set; } = string.Empty;
}
