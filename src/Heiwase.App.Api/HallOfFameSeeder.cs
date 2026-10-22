using System.Text.Json;

using Heiwase.App.Infrastructure.Entities;
using Heiwase.App.Infrastructure.Interface;
using Heiwase.App.Shared.Models;

namespace Heiwase.App.Api;

public sealed record HallOfFameSeedResult(int Created, int Updated);

/// <summary>
/// Imports the static Hall of Fame JSON files into Cosmos DB. The Hungarian and English files describe the same members
/// in the same order, so they are merged by position into one document per member. Ids are deterministic, which makes
/// the import safe to re-run.
/// </summary>
public sealed class HallOfFameSeeder
{
    private const string HungarianFile = "halloffame.json";
    private const string EnglishFile = "hallOfFameEn.json";

    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    private readonly IHallOfFameMemberRepository _repository;

    public HallOfFameSeeder(IHallOfFameMemberRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
    }

    public async Task<HallOfFameSeedResult> SeedAsync(string dataDirectory, CancellationToken cancellationToken)
    {
        HallOfFameDataDto hungarian = await ReadAsync(Path.Combine(dataDirectory, HungarianFile), cancellationToken).ConfigureAwait(false);
        HallOfFameDataDto english = await ReadAsync(Path.Combine(dataDirectory, EnglishFile), cancellationToken).ConfigureAwait(false);

        List<HallOfFameMemberEntity> entities =
        [
            .. Merge(HallOfFameMemberEntity.CompetitorType, hungarian.Competitors, english.Competitors),
            .. Merge(HallOfFameMemberEntity.SenpaiType, hungarian.Senpais, english.Senpais)
        ];

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

    private static IEnumerable<HallOfFameMemberEntity> Merge(
        string type,
        IReadOnlyList<HallOfFameMemberDto> hungarian,
        IReadOnlyList<HallOfFameMemberDto> english)
    {
        if ( hungarian.Count != english.Count )
        {
            throw new InvalidOperationException(
                $"The Hungarian ({hungarian.Count}) and English ({english.Count}) '{type}' lists must have the same length.");
        }

        for ( int i = 0; i < hungarian.Count; i++ )
        {
            if ( !string.Equals(hungarian[ i ].ImagePath, english[ i ].ImagePath, StringComparison.Ordinal) )
            {
                throw new InvalidOperationException(
                    $"The '{type}' entries at position {i} do not match: '{hungarian[ i ].ImagePath}' vs '{english[ i ].ImagePath}'.");
            }

            yield return new HallOfFameMemberEntity
            {
                Id = $"{type}-{i + 1:D3}",
                Type = type,
                Order = i + 1,
                ImagePath = hungarian[ i ].ImagePath,
                Translations = new Dictionary<string, HallOfFameMemberTranslationEntity>
                {
                    [ "hu" ] = ToTranslation(hungarian[ i ]),
                    [ "en" ] = ToTranslation(english[ i ])
                }
            };
        }
    }

    private static HallOfFameMemberTranslationEntity ToTranslation(HallOfFameMemberDto dto) => new( )
    {
        Name = dto.Name,
        Title = dto.Title,
        Description = [ .. dto.Description ]
    };

    private static async Task<HallOfFameDataDto> ReadAsync(string path, CancellationToken cancellationToken)
    {
        await using FileStream stream = File.OpenRead(path);
        return await JsonSerializer.DeserializeAsync<HallOfFameDataDto>(stream, JsonOptions, cancellationToken).ConfigureAwait(false)
            ?? throw new InvalidOperationException($"'{path}' does not contain Hall of Fame data.");
    }
}
