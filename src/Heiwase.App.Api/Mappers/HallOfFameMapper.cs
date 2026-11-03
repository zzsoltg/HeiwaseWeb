using Heiwase.App.Infrastructure.Entities;
using Heiwase.App.Shared.Models;

namespace Heiwase.App.Api.Mappers;

/// <summary>
/// Maps Hall of Fame entities to the DTOs consumed by the Blazor client.
/// </summary>
internal static class HallOfFameMapper
{
    private const string CompetitorType = "competitor";
    private const string SenpaiType = "senpai";

    public static HallOfFameMemberDto ToDto(this HallOfFameMemberEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new HallOfFameMemberDto
        {
            Id = entity.Id,
            Type = entity.Type,
            Order = entity.Order,
            Names = new Dictionary<string, string>(entity.Name),
            Titles = new Dictionary<string, string>(entity.Title),
            Descriptions = entity.Description.ToDictionary(pair => pair.Key, pair => new List<string>(pair.Value)),
            ImagePath = entity.ImagePath
        };
    }

    /// <summary>
    /// Groups the members by their type, preserving the incoming (display) order.
    /// </summary>
    public static HallOfFameDataDto ToDataDto(this IEnumerable<HallOfFameMemberEntity> entities)
    {
        ArgumentNullException.ThrowIfNull(entities);

        var members = entities.Select(ToDto).ToList();

        return new HallOfFameDataDto
        {
            Competitors = [.. members.Where(member => IsType(member, CompetitorType))],
            Senpais = [.. members.Where(member => IsType(member, SenpaiType))]
        };
    }

    private static bool IsType(HallOfFameMemberDto member, string type)
        => string.Equals(member.Type, type, StringComparison.OrdinalIgnoreCase);
}
