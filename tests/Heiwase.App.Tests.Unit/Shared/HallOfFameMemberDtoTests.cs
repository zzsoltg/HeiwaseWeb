using System.Globalization;
using System.Text.Json;

using Heiwase.App.Shared.Models;

namespace Heiwase.App.Tests.Unit.Shared;

public sealed class HallOfFameMemberDtoTests
{
    private static HallOfFameMemberDto CreateDto() => new()
    {
        Names = new() { ["hu"] = "Kovács", ["en"] = "Smith" },
        Titles = new() { ["hu"] = "Mester", ["en"] = "Master" },
        Descriptions = new() { ["hu"] = ["egy", "kettő"], ["en"] = ["one"] },
        ImagePath = "img/members/john.png",
    };

    private static T WithCulture<T>(string culture, Func<T> action)
    {
        var original = CultureInfo.CurrentCulture;
        try
        {
            CultureInfo.CurrentCulture = new CultureInfo(culture);
            return action();
        }
        finally
        {
            CultureInfo.CurrentCulture = original;
        }
    }

    [Theory]
    [InlineData("hu", "Kovács", "Mester")]
    [InlineData("en", "Smith", "Master")]
    public void Name_And_Title_Resolve_For_Current_Culture(string culture, string name, string title)
    {
        var dto = CreateDto();

        Assert.Equal(name, WithCulture(culture, () => dto.Name));
        Assert.Equal(title, WithCulture(culture, () => dto.Title));
    }

    [Fact]
    public void Resolve_Falls_Back_To_English_When_Culture_Is_Missing()
    {
        var dto = CreateDto();

        Assert.Equal("Smith", WithCulture("de", () => dto.Name));
        Assert.Equal(["one"], WithCulture("de", () => dto.Description));
    }

    [Fact]
    public void Resolve_Falls_Back_To_First_Value_When_No_English_Exists()
    {
        HallOfFameMemberDto dto = new() { Names = new() { ["hu"] = "Kovács" } };

        Assert.Equal("Kovács", WithCulture("de", () => dto.Name));
    }

    [Fact]
    public void Resolve_Returns_Empty_Defaults_When_Nothing_Is_Translated()
    {
        HallOfFameMemberDto dto = new();

        Assert.Equal(string.Empty, dto.Name);
        Assert.Equal(string.Empty, dto.Title);
        Assert.Empty(dto.Description);
    }

    [Fact]
    public void Image_Paths_Replace_The_Extension()
    {
        var dto = CreateDto();

        Assert.Equal("img/members/john.avif", dto.AvifPath.Replace('\\', '/'));
        Assert.Equal("img/members/john.webp", dto.WebpPath.Replace('\\', '/'));
        Assert.Equal("img/members/john.jpg", dto.JpgPath.Replace('\\', '/'));
        Assert.Equal("img/members/john.png", dto.PngPath.Replace('\\', '/'));
    }

    [Fact]
    public void Json_Uses_Contract_Names_And_Ignores_Computed_Properties()
    {
        var json = JsonSerializer.Serialize(CreateDto());

        using JsonDocument document = JsonDocument.Parse(json);
        var root = document.RootElement;

        Assert.True(root.TryGetProperty("name", out _));
        Assert.True(root.TryGetProperty("title", out _));
        Assert.True(root.TryGetProperty("imagePath", out _));
        Assert.False(root.TryGetProperty("Name", out _));
        Assert.False(root.TryGetProperty("AvifPath", out _));
    }
}
