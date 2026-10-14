using Azure.Core;
using Azure.Security.KeyVault.Secrets;

using Heiwase.App.Infrastructure.Configuration;

using Microsoft.Extensions.Options;

namespace Heiwase.App.Infrastructure.DataAccess;

internal sealed class KeyVaultCosmosConnectionStringProvider : ICosmosConnectionStringProvider
{
    private readonly SecretClient _secretClient;
    private readonly string _secretName;

    public KeyVaultCosmosConnectionStringProvider(IOptions<CosmosDbOptions> options, TokenCredential credential)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(credential);

        CosmosDbOptions settings = options.Value;
        _secretClient = new SecretClient(new Uri(settings.KeyVaultUri), credential);
        _secretName = settings.ConnectionStringSecretName;
    }

    public async Task<string> GetConnectionStringAsync(CancellationToken cancellationToken)
    {
        KeyVaultSecret secret = await _secretClient.GetSecretAsync(_secretName, cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return secret.Value;
    }
}
