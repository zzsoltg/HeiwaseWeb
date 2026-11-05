using Heiwase.App.Infrastructure.Entities;
using Heiwase.App.Shared.Models;

namespace Heiwase.App.Api.Mappers;

internal static class ArticleMapper
{
    public static ArticleDto ToDto(this ArticleEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new ArticleDto
        {
            Id = entity.Id,
            Title = entity.Title,
            MarkdownContent = entity.MarkdownContent,
            CreatedAt = entity.CreatedAt,
            IsPublished = entity.IsPublished
        };
    }

    public static ArticleEntity ToEntity(this ArticleDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        return new ArticleEntity
        {
            Id = string.Empty,
            Title = dto.Title,
            MarkdownContent = dto.MarkdownContent,
            CreatedAt = dto.CreatedAt == default ? DateTime.UtcNow : dto.CreatedAt.ToUniversalTime(),
            IsPublished = dto.IsPublished
        };
    }
}
