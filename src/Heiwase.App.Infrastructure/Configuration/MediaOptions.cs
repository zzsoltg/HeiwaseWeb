using System.ComponentModel.DataAnnotations;

namespace Heiwase.App.Infrastructure.Configuration;

public sealed class MediaOptions
{
    public const string SectionName = "Media";

    [Required]
    public string AccountName { get; set; } = string.Empty;

    [Required]
    public string ContainerName { get; set; } = "public-media";

    /// <summary>Optional blob service endpoint override (e.g. Azurite). Defaults to https://{AccountName}.blob.core.windows.net.</summary>
    [Url]
    public string? BlobEndpoint { get; set; }

    [Range(1, 52428800)]
    public int MaxFileSizeBytes { get; set; } = 5 * 1024 * 1024;
}
