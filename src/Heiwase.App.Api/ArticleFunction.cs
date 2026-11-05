using System.Text.Json;

using Heiwase.App.Api.Mappers;
using Heiwase.App.Infrastructure.Interface;
using Heiwase.App.Shared.Models;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;

namespace Heiwase.App.Api;

public sealed class ArticleFunction
{
    private readonly IArticleRepository _repository;

    public ArticleFunction(IArticleRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
    }

    [Function("GetArticles")]
    public async Task<IActionResult> GetAllAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "articles")]
        HttpRequest req)
    {
        ArgumentNullException.ThrowIfNull(req);

        var articles = await _repository.GetPublishedAsync(req.HttpContext.RequestAborted).ConfigureAwait(false);

        return new OkObjectResult(articles.Select(ArticleMapper.ToDto).ToList());
    }

    [Function("GetArticleById")]
    public async Task<IActionResult> GetByIdAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "articles/{id}")]
        HttpRequest req,
        string id)
    {
        ArgumentNullException.ThrowIfNull(req);

        var article = await _repository.GetByIdAsync(id, req.HttpContext.RequestAborted).ConfigureAwait(false);

        return article is { IsPublished: true }
            ? new OkObjectResult(article.ToDto())
            : new NotFoundResult();
    }

    // TODO: Restrict to administrators ([Authorize(Roles = "Administrator")]) once the admin portal exists.
    [Function("CreateArticle")]
    public async Task<IActionResult> CreateAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "articles")]
        HttpRequest req)
    {
        ArgumentNullException.ThrowIfNull(req);

        ArticleDto? dto;
        try
        {
            dto = await req.ReadFromJsonAsync<ArticleDto>(cancellationToken: req.HttpContext.RequestAborted).ConfigureAwait(false);
        }
        catch (JsonException)
        {
            return new BadRequestObjectResult("Invalid JSON body.");
        }

        if (dto is null || string.IsNullOrWhiteSpace(dto.Title) || string.IsNullOrWhiteSpace(dto.MarkdownContent))
        {
            return new BadRequestObjectResult("Title and markdownContent are required.");
        }

        var entity = dto.ToEntity();
        await _repository.AddAsync(entity, req.HttpContext.RequestAborted).ConfigureAwait(false);

        return new CreatedResult($"/api/articles/{entity.Id}", entity.ToDto());
    }
}
