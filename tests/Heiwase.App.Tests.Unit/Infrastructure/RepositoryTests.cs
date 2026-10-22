using System.Net;

using Heiwase.App.Infrastructure.DataAccess;
using Heiwase.App.Infrastructure.Entities;
using Heiwase.App.Infrastructure.Repository;

using Microsoft.Azure.Cosmos;

using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace Heiwase.App.Tests.Unit.Infrastructure;

public sealed class RepositoryTests
{
    public sealed class TestEntity : BaseEntity
    {
        public string Value { get; set; } = string.Empty;
    }

    private sealed class TestRepository(ICosmosContainerProvider provider, string containerName = "items")
        : Repository<TestEntity>(provider, containerName);

    private readonly Container _container = Substitute.For<Container>( );
    private readonly ICosmosContainerProvider _provider = Substitute.For<ICosmosContainerProvider>( );

    public RepositoryTests( )
    {
        _provider.GetContainerAsync("items", Arg.Any<CancellationToken>( )).Returns(_container);
    }

    private TestRepository CreateSut( ) => new(_provider);

    private static CosmosException CosmosError(HttpStatusCode status)
        => new("error", status, 0, "activity", 0);

    [Fact]
    public void Constructor_Validates_Arguments()
    {
        Assert.Throws<ArgumentNullException>(( ) => new TestRepository(null!));
        Assert.Throws<ArgumentException>(( ) => new TestRepository(_provider, " "));
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public async Task GetById_Rejects_Blank_Id(string id)
        => await Assert.ThrowsAsync<ArgumentException>(( ) => CreateSut( ).GetByIdAsync(id));

    [Fact]
    public async Task GetById_Returns_Entity_Using_Id_As_Partition_Key()
    {
        TestEntity entity = new( ) { Id = "42" };
        ItemResponse<TestEntity> response = Substitute.For<ItemResponse<TestEntity>>( );
        response.Resource.Returns(entity);
        _container.ReadItemAsync<TestEntity>("42", new PartitionKey("42"), cancellationToken: Arg.Any<CancellationToken>( ))
            .Returns(response);

        TestEntity? result = await CreateSut( ).GetByIdAsync("42");

        Assert.Same(entity, result);
    }

    [Fact]
    public async Task GetById_Returns_Null_When_Item_Is_Not_Found()
    {
        _container.ReadItemAsync<TestEntity>(default!, default, default, default)
            .ReturnsForAnyArgs(Task.FromException<ItemResponse<TestEntity>>(CosmosError(HttpStatusCode.NotFound)));

        Assert.Null(await CreateSut( ).GetByIdAsync("missing"));
    }

    [Fact]
    public async Task GetById_Propagates_Other_Cosmos_Errors()
    {
        _container.ReadItemAsync<TestEntity>(default!, default, default, default)
            .ReturnsForAnyArgs(Task.FromException<ItemResponse<TestEntity>>(CosmosError(HttpStatusCode.TooManyRequests)));

        CosmosException ex = await Assert.ThrowsAsync<CosmosException>(( ) => CreateSut( ).GetByIdAsync("1"));

        Assert.Equal(HttpStatusCode.TooManyRequests, ex.StatusCode);
    }

    [Fact]
    public async Task Add_Rejects_Null_Entity()
        => await Assert.ThrowsAsync<ArgumentNullException>(( ) => CreateSut( ).AddAsync(null!));

    [Fact]
    public async Task Add_Generates_Id_When_Missing()
    {
        TestEntity entity = new( );

        await CreateSut( ).AddAsync(entity);

        Assert.True(Guid.TryParse(entity.Id, out _));
        await _container.Received(1).CreateItemAsync(
            entity,
            new PartitionKey(entity.Id),
            Arg.Any<ItemRequestOptions>( ),
            Arg.Any<CancellationToken>( ));
    }

    [Fact]
    public async Task Add_Keeps_Existing_Id()
    {
        TestEntity entity = new( ) { Id = "keep" };

        await CreateSut( ).AddAsync(entity);

        Assert.Equal("keep", entity.Id);
        await _container.Received(1).CreateItemAsync(
            entity, new PartitionKey("keep"), Arg.Any<ItemRequestOptions>( ), Arg.Any<CancellationToken>( ));
    }

    [Fact]
    public async Task Update_Validates_Entity_And_Id()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(( ) => CreateSut( ).UpdateAsync(null!));
        await Assert.ThrowsAsync<ArgumentException>(( ) => CreateSut( ).UpdateAsync(new TestEntity( )));
    }

    [Fact]
    public async Task Update_Replaces_Item_By_Id()
    {
        TestEntity entity = new( ) { Id = "7" };

        await CreateSut( ).UpdateAsync(entity);

        await _container.Received(1).ReplaceItemAsync(
            entity, "7", new PartitionKey("7"), Arg.Any<ItemRequestOptions>( ), Arg.Any<CancellationToken>( ));
    }

    [Fact]
    public async Task Delete_Validates_Entity_And_Id()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(( ) => CreateSut( ).DeleteAsync(null!));
        await Assert.ThrowsAsync<ArgumentException>(( ) => CreateSut( ).DeleteAsync(new TestEntity( )));
    }

    [Fact]
    public async Task Delete_Removes_Item_By_Id()
    {
        TestEntity entity = new( ) { Id = "9" };

        await CreateSut( ).DeleteAsync(entity);

        await _container.Received(1).DeleteItemAsync<TestEntity>(
            "9", new PartitionKey("9"), Arg.Any<ItemRequestOptions>( ), Arg.Any<CancellationToken>( ));
    }

    [Fact]
    public async Task Operations_Surface_Provider_Failures()
    {
        _provider.GetContainerAsync(Arg.Any<string>( ), Arg.Any<CancellationToken>( ))
            .ThrowsAsync(new InvalidOperationException("no container"));

        await Assert.ThrowsAsync<InvalidOperationException>(( ) => CreateSut( ).AddAsync(new TestEntity( )));
    }
}
