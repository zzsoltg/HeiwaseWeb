using Bunit;

using Heiwase.App.Blazor.Components.Shared.Dialogs.CompetitorResultsDialog;
using Heiwase.App.Blazor.Components.Shared.Dialogs.PictureShowDialog;
using Heiwase.App.Blazor.Components.Shared.Dialogs.SenpaiResultsDialog;
using Heiwase.App.Blazor.Components.Shared.StickyCta;
using Heiwase.App.Tests.Blazor.Support;

namespace Heiwase.App.Tests.Blazor.Shared;

/// <summary>
/// Renders every content-only component (dialogs, hero, timetable, footer ...) in both cultures,
/// so a broken resource key, markup error or missing localized variant is caught.
/// </summary>
public sealed class StaticContentSmokeTests : SiteTestContext
{
    private static readonly string[] _excludedSuffixes = ["PictureShowDialog", "CompetitorResultsDialog", "SenpaiResultsDialog"];

    private static readonly string[] _namespacePrefixes =
    [
        "Heiwase.App.Blazor.Components.Shared.Dialogs.",
        "Heiwase.App.Blazor.Components.Shared.SiteFooter.",
        "Heiwase.App.Blazor.Components.Sections.Home.HeroSection.",
        "Heiwase.App.Blazor.Components.Sections.Home.SocialSection.",
        "Heiwase.App.Blazor.Components.Sections.Home.TimetableSection.",
    ];

    public static TheoryData<string, string> Components()
    {
        TheoryData<string, string> data = [];
        IEnumerable<Type> types = typeof(PictureShowDialog).Assembly.GetTypes()
            .Where(t => typeof(Microsoft.AspNetCore.Components.IComponent).IsAssignableFrom(t)
                && !t.IsAbstract
                && !t.Name.Contains('_', StringComparison.Ordinal)
                && t.Namespace is not null
                && _namespacePrefixes.Any(p => t.Namespace.StartsWith(p, StringComparison.Ordinal))
                && !_excludedSuffixes.Contains(t.Name))
            .OrderBy(t => t.FullName, StringComparer.Ordinal);

        foreach (Type type in types)
        {
            data.Add(type.AssemblyQualifiedName!, Hungarian);
            data.Add(type.AssemblyQualifiedName!, English);
        }

        return data;
    }

    [Fact]
    public void Component_Discovery_Finds_All_Static_Components()
        => Assert.True(Components().Count >= 30);

    [Theory]
    [MemberData(nameof(Components))]
    public void Renders_Content_In_Each_Culture(string typeName, string culture)
    {
        UseCulture(culture);
        Type type = Type.GetType(typeName, throwOnError: true)!;

        IRenderedComponent<Microsoft.AspNetCore.Components.IComponent> cut = Render<Microsoft.AspNetCore.Components.IComponent>(
            builder =>
            {
                builder.OpenComponent(0, type);
                builder.CloseComponent();
            });

        Assert.False(string.IsNullOrWhiteSpace(cut.Markup));
    }

    [Theory]
    [InlineData(Hungarian, "CSATLAKOZZ MÉG MA!")]
    [InlineData(English, "JOIN US TODAY!")]
    public void StickyCta_Is_Localized(string culture, string text)
    {
        UseCulture(culture);

        IRenderedComponent<StickyCta> cut = Render<StickyCta>();

        Assert.Contains(text, cut.Markup, StringComparison.Ordinal);
    }

    [Fact]
    public void PictureShowDialog_Shows_The_Image_With_Its_Alt_Text()
    {
        IRenderedComponent<PictureShowDialog> cut = Render<PictureShowDialog>(p => p
            .Add(c => c.ImagePath, "img/a.jpg")
            .Add(c => c.AltText, "Alt szöveg"));

        Assert.Contains("img/a.jpg", cut.Markup, StringComparison.Ordinal);
        Assert.Contains("Alt szöveg", cut.Markup, StringComparison.Ordinal);
    }

    [Theory]
    [InlineData(Hungarian)]
    [InlineData(English)]
    public void CompetitorResultsDialog_Lists_The_Member_Results(string culture)
    {
        UseCulture(culture);
        var member = HallOfFameTestData.Member("c1", "Kiss Pista", "Kumite", "Arany 2024", "Ezüst 2023");

        IRenderedComponent<CompetitorResultsDialog> cut = Render<CompetitorResultsDialog>(p => p.Add(c => c.Member, member));

        Assert.Contains("Arany 2024", cut.Markup, StringComparison.Ordinal);
        Assert.Contains("Ezüst 2023", cut.Markup, StringComparison.Ordinal);
    }

    [Fact]
    public void CompetitorResultsDialog_Renders_Without_A_Member()
    {
        IRenderedComponent<CompetitorResultsDialog> cut = Render<CompetitorResultsDialog>();

        Assert.NotNull(cut.Markup);
    }

    [Theory]
    [InlineData("1st dan", "Sensei")]
    [InlineData("3rd kyu", "Senpai")]
    public void SenpaiResultsDialog_Picks_The_Role_From_The_Title(string title, string role)
    {
        var member = HallOfFameTestData.Member("s1", "Nagy Anna", title, "Vizsga 2022");

        IRenderedComponent<SenpaiResultsDialog> cut = Render<SenpaiResultsDialog>(p => p.Add(c => c.Member, member));

        Assert.Contains(role, cut.Markup, StringComparison.Ordinal);
        Assert.Contains("Vizsga 2022", cut.Markup, StringComparison.Ordinal);
    }

    [Fact]
    public void SenpaiResultsDialog_Renders_Without_A_Member()
    {
        IRenderedComponent<SenpaiResultsDialog> cut = Render<SenpaiResultsDialog>();

        Assert.Contains("Senpai", cut.Markup, StringComparison.Ordinal);
    }
}
