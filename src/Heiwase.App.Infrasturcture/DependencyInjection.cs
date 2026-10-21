using Azure.Core;
using Azure.Identity;

using Heiwase.App.Infrastructure.Configuration;
using Heiwase.App.Infrastructure.DataAccess;
using Heiwase.App.Infrastructure.Interface;
using Heiwase.App.Infrastructure.Repository;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Heiwase.App.Infrastructure;

public static class DependencyInjection
{
    /// <summary>
    /// Registers the Cosmos DB repositories. The connection string is read from Azure Key Vault
    /// using <see cref="DefaultAzureCredential"/> (managed identity in Azure, developer login locally).
    /// </summary>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        services.AddOptions<CosmosDbOptions>( )
            .Bind(configuration.GetSection(CosmosDbOptions.SectionName))
            .ValidateDataAnnotations( )
            .ValidateOnStart( );

        services.TryAddSingleton<TokenCredential>(_ => new DefaultAzureCredential( ));
        services.AddSingleton<ICosmosConnectionStringProvider, KeyVaultCosmosConnectionStringProvider>( );
        services.AddSingleton<ICosmosContainerProvider, CosmosContainerProvider>( );

        services.AddSingleton<IArticleRepository, ArticleRepository>( );
        services.AddSingleton<IHallOfFameMemberRepository, HallOfFameMemberRepository>( );
        services.AddSingleton<IApplicationFormRepository, ApplicationFormRepository>( );

        return services;
    }
}
