using System.Globalization;
using System.Text.Json.Serialization;

namespace Heiwase.App.Shared.Models;

/// <summary>
/// A Hall of Fame member. The translated fields hold one value per language code (<c>hu</c>, <c>en</c>); the
/// <see cref="Name"/>, <see cref="Title"/> and <see cref="Description"/> shortcuts resolve them for the current culture.
/// </summary>
public class HallOfFameMemberDto
{
    private const string DefaultLanguage = "en";

    [JsonPropertyName("id")]
    public string Id { get; set; } = String.Empty;

    [JsonPropertyName("type")]
    public string Type { get; set; } = String.Empty;

    [JsonPropertyName("order")]
    public int Order { get; set; }

    [JsonPropertyName("name")]
    public Dictionary<string, string> Names { get; set; } = [ ];

    [JsonPropertyName("title")]
    public Dictionary<string, string> Titles { get; set; } = [ ];

    [JsonPropertyName("description")]
    public Dictionary<string, List<string>> Descriptions { get; set; } = [ ];

    [JsonPropertyName("imagePath")]
    public string ImagePath { get; set; } = String.Empty;

    [JsonIgnore]
    public string Name => Resolve(Names) ?? String.Empty;

    [JsonIgnore]
    public string Title => Resolve(Titles) ?? String.Empty;

    [JsonIgnore]
    public List<string> Description => Resolve(Descriptions) ?? [ ];

    [JsonIgnore]
    public string AvifPath => Path.ChangeExtension(ImagePath, ".avif");

    [JsonIgnore]
    public string WebpPath => Path.ChangeExtension(ImagePath, ".webp");

    [JsonIgnore]
    public string JpgPath => Path.ChangeExtension(ImagePath, ".jpg");

    [JsonIgnore]
    public string PngPath => Path.ChangeExtension(ImagePath, ".png");

    private static T? Resolve<T>(Dictionary<string, T> values)
    {
        string language = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;

        if ( values.TryGetValue(language, out T? value) || values.TryGetValue(DefaultLanguage, out value) )
        {
            return value;
        }

        return values.Values.FirstOrDefault( );
    }
}
