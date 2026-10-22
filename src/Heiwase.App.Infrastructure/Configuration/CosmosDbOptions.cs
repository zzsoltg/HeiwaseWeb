using System.ComponentModel.DataAnnotations;

namespace Heiwase.App.Infrastructure.Configuration;

public sealed class CosmosDbOptions
{
    public const string SectionName = "CosmosDb";

    [Required, Url]
    public string KeyVaultUri { get; set; } = string.Empty;

    [Required]
    public string ConnectionStringSecretName { get; set; } = "CosmosDb-ConnectionString";

    [Required]
    public string DatabaseName { get; set; } = "HeiwaseDb";

    [Required]
    public string ArticlesContainerName { get; set; } = "articles";

    [Required]
    public string HallOfFameContainerName { get; set; } = "hallOfFame";

    [Required]
    public string ApplicationsContainerName { get; set; } = "applications";
}
