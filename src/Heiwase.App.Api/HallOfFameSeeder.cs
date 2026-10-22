using System.Text.Json;

using Heiwase.App.Infrastructure.Entities;
using Heiwase.App.Infrastructure.Interface;
using Heiwase.App.Shared.Models;

namespace Heiwase.App.Api;

public sealed record HallOfFameSeedResult(int Created, int Updated);

/// <summary>
/// Imports the static Hall of Fame JSON file into Cosmos DB. Every member already carries its own id, so the import is
/// safe to re-run: existing documents are replaced, missing ones are created.
/// </summary>
public sealed class HallOfFameSeeder
{
    private const string DataFile = "halloffame.json";

    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    private readonly IHallOfFameMemberRepository _repository;

    public HallOfFameSeeder(IHallOfFameMemberRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
    }

    public async Task<HallOfFameSeedResult> SeedAsync(string dataDirectory, CancellationToken cancellationToken)
    {
        HallOfFameDataDto data = await ReadAsync(Path.Combine(dataDirectory, DataFile), cancellationToken).ConfigureAwait(false);

        List<HallOfFameMemberEntity> entities =
        [
            .. data.Competitors.Select(member => ToEntity(member, HallOfFameMemberEntity.CompetitorType)),
            .. data.Senpais.Select(member => ToEntity(member, HallOfFameMemberEntity.SenpaiType))
        ];

        if ( entities.Select(entity => entity.Id).Distinct(StringComparer.Ordinal).Count( ) != entities.Count )
        {
            throw new InvalidOperationException($"'{DataFile}' contains duplicate member ids.");
        }

        int created = 0;
        int updated = 0;
        foreach ( HallOfFameMemberEntity entity in entities )
        {
            HallOfFameMemberEntity? existing = await _repository.GetByIdAsync(entity.Id, cancellationToken).ConfigureAwait(false);
            if ( existing is null )
            {
                await _repository.AddAsync(entity, cancellationToken).ConfigureAwait(false);
                created++;
            }
            else
            {
                await _repository.UpdateAsync(entity, cancellationToken).ConfigureAwait(false);
                updated++;
            }
        }

        return new HallOfFameSeedResult(created, updated);
    }

    private static HallOfFameMemberEntity ToEntity(HallOfFameMemberDto dto, string type)
    {
        if ( string.IsNullOrWhiteSpace(dto.Id) )
        {
            throw new InvalidOperationException($"A '{type}' entry in '{DataFile}' has no id.");
        }

        return new HallOfFameMemberEntity
        {
            Id = dto.Id,
            Type = type,
            Order = dto.Order,
            ImagePath = dto.ImagePath,
            Name = new Dictionary<string, string>(dto.Names),
            Title = new Dictionary<string, string>(dto.Titles),
            Description = dto.Descriptions.ToDictionary(pair => pair.Key, pair => new List<string>(pair.Value))
        };
    }

    private static async Task<HallOfFameDataDto> ReadAsync(string path, CancellationToken cancellationToken)
    {
        await using FileStream stream = File.OpenRead(path);
        return await JsonSerializer.DeserializeAsync<HallOfFameDataDto>(stream, JsonOptions, cancellationToken).ConfigureAwait(false)
            ?? throw new InvalidOperationException($"'{path}' does not contain Hall of Fame data.");
    }
}
