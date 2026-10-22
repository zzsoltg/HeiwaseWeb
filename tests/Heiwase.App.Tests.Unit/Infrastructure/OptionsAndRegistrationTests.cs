using System.ComponentModel.DataAnnotations;

using Azure.Core;

using Heiwase.App.Infrastructure;
using Heiwase.App.Infrastructure.Configuration;
using Heiwase.App.Infrastructure.DataAccess;
using Heiwase.App.Infrastructure.Interface;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using NSubstitute;

namespace Heiwase.App.Tests.Unit.Infrastructure;

public sealed class OptionsAndRegistrationTests
{
    private static bool IsValid(object model, out List<ValidationResult> results)
    {
        results = [ ];
        return Validator.TryValidateObject(model, new ValidationContext(model), results, validateAllProperties: true);
    }

    private static IConfiguration Config(Dictionary<string, string?> values)
        => new ConfigurationBuilder( ).AddInMemoryCollection(values).Build( );

    [Fact]
    public void CosmosDbOptions_Defaults_Are_Valid_When_Required_Values_Are_Set()
    {
        CosmosDbOptions options = new( ) { KeyVaultUri = "https://vault.vault.azure.net/" };

        Assert.True(IsValid(options, out _));
        Assert.Equal("HeiwaseDb", options.DatabaseName);
    }

    [Theory]
    [InlineData("")]
    [InlineData("not-a-url")]
    public void CosmosDbOptions_Requires_A_Valid_KeyVault_Uri(string uri)
        => Assert.False(IsValid(new CosmosDbOptions { KeyVaultUri = uri }, out _));

    [Fact]
    public void MediaOptions_Requires_Account_Name()
        => Assert.False(IsValid(new MediaOptions( ), out _));

    [Theory]
    [InlineData(0, false)]
    [InlineData(1, true)]
    [InlineData(52428800, true)]
    [InlineData(52428801, false)]
    public void MediaOptions_Limits_File_Size_Range(int size, bool expected)
        => Assert.Equal(expected, IsValid(new MediaOptions { AccountName = "a", MaxFileSizeBytes = size }, out _));

    [Fact]
    public void MediaOptions_Rejects_Invalid_Endpoint()
        => Assert.False(IsValid(new MediaOptions { AccountName = "a", BlobEndpoint = "nope" }, out _));

    [Fact]
    public void AddInfrastructure_Rejects_Null_Arguments()
    {
        Assert.Throws<ArgumentNullException>(( ) => DependencyInjection.AddInfrastructure(null!, Config([ ])));
        Assert.Throws<ArgumentNullException>(( ) => new ServiceCollection( ).AddInfrastructure(null!));
    }

    [Fact]
    public void AddInfrastructure_Registers_Services_As_Singletons()
    {
        ServiceCollection services = new( );

        services.AddInfrastructure(Config([ ]));

        foreach ( Type type in new[ ]
        {
            typeof(IArticleRepository), typeof(IHallOfFameMemberRepository), typeof(IApplicationFormRepository),
            typeof(IMediaService), typeof(ICosmosContainerProvider), typeof(ICosmosConnectionStringProvider),
        } )
        {
            ServiceDescriptor descriptor = Assert.Single(services, d => d.ServiceType == type);
            Assert.Equal(ServiceLifetime.Singleton, descriptor.Lifetime);
        }
    }

    [Fact]
    public void AddInfrastructure_Does_Not_Override_A_Preregistered_Credential()
    {
        TokenCredential credential = Substitute.For<TokenCredential>( );
        ServiceCollection services = new( );
        services.AddSingleton(credential);

        services.AddInfrastructure(Config([ ]));

        using ServiceProvider provider = services.BuildServiceProvider( );
        Assert.Same(credential, provider.GetRequiredService<TokenCredential>( ));
    }

    [Fact]
    public void Options_Validation_Fails_For_Invalid_Configuration()
    {
        ServiceCollection services = new( );
        services.AddInfrastructure(Config([ ]));
        using ServiceProvider provider = services.BuildServiceProvider( );

        Assert.Throws<OptionsValidationException>(( ) => provider.GetRequiredService<IOptions<CosmosDbOptions>>( ).Value);
    }

    [Fact]
    public void Resolves_All_Services_With_Valid_Configuration()
    {
        ServiceCollection services = new( );
        services.AddSingleton(Substitute.For<TokenCredential>( ));
        services.AddInfrastructure(Config(new( )
        {
            [ "CosmosDb:KeyVaultUri" ] = "https://vault.vault.azure.net/",
            [ "Media:AccountName" ] = "acct",
        }));
        using ServiceProvider provider = services.BuildServiceProvider( );

        Assert.NotNull(provider.GetRequiredService<IArticleRepository>( ));
        Assert.NotNull(provider.GetRequiredService<IHallOfFameMemberRepository>( ));
        Assert.NotNull(provider.GetRequiredService<IApplicationFormRepository>( ));
        Assert.NotNull(provider.GetRequiredService<IMediaService>( ));
    }

    [Fact]
    public async Task ContainerProvider_Rejects_Blank_Container_Name()
    {
        using CosmosContainerProvider sut = new(
            Substitute.For<ICosmosConnectionStringProvider>( ),
            Options.Create(new CosmosDbOptions( )));

        await Assert.ThrowsAsync<ArgumentException>(( ) => sut.GetContainerAsync(" ", CancellationToken.None));
    }

    [Fact]
    public void ContainerProvider_And_KeyVault_Provider_Reject_Null_Arguments()
    {
        Assert.Throws<ArgumentNullException>(( ) => new CosmosContainerProvider(null!, Options.Create(new CosmosDbOptions( ))));
        Assert.Throws<ArgumentNullException>(( ) => new CosmosContainerProvider(Substitute.For<ICosmosConnectionStringProvider>( ), null!));
        Assert.Throws<ArgumentNullException>(( ) => new KeyVaultCosmosConnectionStringProvider(null!, Substitute.For<TokenCredential>( )));
        Assert.Throws<ArgumentNullException>(( ) => new KeyVaultCosmosConnectionStringProvider(Options.Create(new CosmosDbOptions( )), null!));
    }
}
