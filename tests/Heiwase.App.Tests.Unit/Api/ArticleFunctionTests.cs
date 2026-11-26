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
