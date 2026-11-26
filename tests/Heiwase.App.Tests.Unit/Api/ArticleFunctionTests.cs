using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

using Heiwase.App.Api;
using Heiwase.App.Api.Services;
using Heiwase.App.Infrastructure.Entities;
using Heiwase.App.Infrastructure.Interface;
using Heiwase.App.Shared.Models;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

using NSubstitute;

namespace Heiwase.App.Tests.Unit.Api;

public sealed class ArticleFunctionTests
{
    private const string Issuer = "https://issuer.example.test";
    private const string Audience = "article-api";
    private static readonly SymmetricSecurityKey SigningKey = new(Encoding.UTF8.GetBytes("a-test-signing-key-with-at-least-32-bytes"));

    [Theory]
    [InlineData(null)]
    [InlineData("invalid-token")]
    public async Task Create_Rejects_Missing_Or_Invalid_Bearer_Token(string? token)
    {
        var repository = Substitute.For<IArticleRepository>();
        var request = CreateRequest(token);
        request.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(
            [new Claim("roles", "Administrator")], "untrusted", "name", "roles"));

        var result = await new ArticleFunction(repository).CreateAsync(request);

        Assert.IsType<UnauthorizedResult>(result);
        await repository.DidNotReceive().AddAsync(Arg.Any<ArticleEntity>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Create_Rejects_Expired_Administrator_Token()
    {
        var repository = Substitute.For<IArticleRepository>();
        var request = CreateRequest(CreateToken("Administrator", DateTime.UtcNow.AddMinutes(-5)));

        var result = await new ArticleFunction(repository).CreateAsync(request);

        Assert.IsType<UnauthorizedResult>(result);
        await repository.DidNotReceive().AddAsync(Arg.Any<ArticleEntity>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Create_Rejects_Authenticated_Non_Administrator()
    {
        var repository = Substitute.For<IArticleRepository>();
        var request = CreateRequest(CreateToken("Member", DateTime.UtcNow.AddMinutes(5)));

        var result = await new ArticleFunction(repository).CreateAsync(request);

        Assert.IsType<ForbidResult>(result);
        await result.ExecuteResultAsync(new ActionContext { HttpContext = request.HttpContext });
        Assert.Equal(StatusCodes.Status403Forbidden, request.HttpContext.Response.StatusCode);
        await repository.DidNotReceive().AddAsync(Arg.Any<ArticleEntity>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Create_Allows_Authenticated_Administrator()
    {
        var repository = Substitute.For<IArticleRepository>();
        var request = CreateRequest(CreateToken("Administrator", DateTime.UtcNow.AddMinutes(5)));

        var result = await new ArticleFunction(repository).CreateAsync(request);

        Assert.IsType<CreatedResult>(result);
        await repository.Received(1).AddAsync(
            Arg.Is<ArticleEntity>(article => article.Title == "News" && article.MarkdownContent == "Content"),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetById_Returns_Persisted_Seo_Metadata()
    {
        var repository = Substitute.For<IArticleRepository>();
        var article = JsonSerializer.Deserialize<ArticleEntity>(
            """{"id":"article-1","title":"News","markdownContent":"Text","isPublished":true,"summary":"A summary","coverImageUrl":"https://example.test/cover.png","updatedAt":"2026-01-02T03:04:05Z"}""")!;
        repository.GetByIdAsync("article-1", Arg.Any<CancellationToken>()).Returns(article);

        var result = Assert.IsType<OkObjectResult>(await new ArticleFunction(repository).GetByIdAsync(CreateRequest(null), "article-1"));
        using var json = JsonDocument.Parse(JsonSerializer.Serialize(result.Value));

        Assert.Equal("A summary", json.RootElement.GetProperty("summary").GetString());
        Assert.Equal("https://example.test/cover.png", json.RootElement.GetProperty("coverImageUrl").GetString());
        Assert.Equal(DateTime.Parse("2026-01-02T03:04:05Z", null, System.Globalization.DateTimeStyles.RoundtripKind), json.RootElement.GetProperty("updatedAt").GetDateTime());
    }

    [Fact]
    public async Task Create_Stores_Seo_Metadata_And_Sets_Server_Update_Time()
    {
        var repository = Substitute.For<IArticleRepository>();
        var request = CreateRequest(CreateToken("Administrator", DateTime.UtcNow.AddMinutes(5)));
        request.Body = new MemoryStream(Encoding.UTF8.GetBytes(
            """{"title":"News","markdownContent":"Text","summary":"A summary","coverImageUrl":"https://example.test/cover.png","updatedAt":"2000-01-01T00:00:00Z"}"""));
        var before = DateTime.UtcNow;

        var result = Assert.IsType<CreatedResult>(await new ArticleFunction(repository).CreateAsync(request));
        using var json = JsonDocument.Parse(JsonSerializer.Serialize(result.Value));

        Assert.Equal("A summary", json.RootElement.GetProperty("summary").GetString());
        Assert.Equal("https://example.test/cover.png", json.RootElement.GetProperty("coverImageUrl").GetString());
        Assert.InRange(json.RootElement.GetProperty("updatedAt").GetDateTime(), before, DateTime.UtcNow);
        await repository.Received(1).AddAsync(
            Arg.Is<ArticleEntity>(article => article.Summary == "A summary"
                && article.CoverImageUrl == "https://example.test/cover.png"
                && article.UpdatedAt >= before),
            Arg.Any<CancellationToken>());
    }

    [Theory]
    [InlineData("http://example.test/cover.png")]
    [InlineData("javascript:alert(1)")]
    [InlineData("https://user:password@example.test/cover.png")]
    public async Task Create_Rejects_Unsafe_Cover_Image_Url(string imageUrl)
    {
        var repository = Substitute.For<IArticleRepository>();
        var request = CreateRequest(CreateToken("Administrator", DateTime.UtcNow.AddMinutes(5)));
        request.Body = new MemoryStream(JsonSerializer.SerializeToUtf8Bytes(
            new { title = "News", markdownContent = "Text", coverImageUrl = imageUrl }));

        var result = await new ArticleFunction(repository).CreateAsync(request);

        Assert.IsType<BadRequestObjectResult>(result);
        await repository.DidNotReceive().AddAsync(Arg.Any<ArticleEntity>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetById_Does_Not_Invent_Update_Time_For_Legacy_Article()
    {
        var repository = Substitute.For<IArticleRepository>();
        repository.GetByIdAsync("legacy", Arg.Any<CancellationToken>())
            .Returns(new ArticleEntity { Id = "legacy", Title = "Old", IsPublished = true, CreatedAt = new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc) });

        var result = Assert.IsType<OkObjectResult>(await new ArticleFunction(repository).GetByIdAsync(CreateRequest(null), "legacy"));
        using var json = JsonDocument.Parse(JsonSerializer.Serialize(result.Value));

        Assert.Equal(JsonValueKind.Null, json.RootElement.GetProperty("updatedAt").ValueKind);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task Administrator_Guard_Does_Not_Execute_Unauthorized_Action(bool authenticated)
    {
        var token = authenticated ? CreateToken("Member", DateTime.UtcNow.AddMinutes(5)) : null;
        var request = CreateRequest(token);
        var executed = false;

        var result = await AdministratorAuthorization.ExecuteAsync(request, () =>
        {
            executed = true;
            return Task.FromResult<IActionResult>(new OkResult());
        });

        if (authenticated)
        {
            Assert.IsType<ForbidResult>(result);
        }
        else
        {
            Assert.IsType<UnauthorizedResult>(result);
        }

        Assert.False(executed);
    }

    [Fact]
    public async Task Administrator_Guard_Returns_Protected_Action_Result()
    {
        var request = CreateRequest(CreateToken("Administrator", DateTime.UtcNow.AddMinutes(5)));
        var expected = new OkObjectResult("protected");

        var result = await AdministratorAuthorization.ExecuteAsync(
            request, () => Task.FromResult<IActionResult>(expected));

        Assert.Same(expected, result);
    }

    private static HttpRequest CreateRequest(string? token)
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
        {
            options.MapInboundClaims = false;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = Issuer,
                ValidAudience = Audience,
                IssuerSigningKey = SigningKey,
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                RoleClaimType = "roles"
            };
        });

        var context = new DefaultHttpContext { RequestServices = services.BuildServiceProvider() };
        context.Request.ContentType = "application/json";
        context.Request.Body = new MemoryStream(JsonSerializer.SerializeToUtf8Bytes(
            new ArticleDto { Title = "News", MarkdownContent = "Content" }));
        if (token is not null)
        {
            context.Request.Headers.Authorization = $"Bearer {token}";
        }

        return context.Request;
    }

    private static string CreateToken(string role, DateTime expires)
    {
        var token = new JwtSecurityToken(
            Issuer,
            Audience,
            [new Claim("roles", role)],
            expires: expires,
            signingCredentials: new SigningCredentials(SigningKey, SecurityAlgorithms.HmacSha256));
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
