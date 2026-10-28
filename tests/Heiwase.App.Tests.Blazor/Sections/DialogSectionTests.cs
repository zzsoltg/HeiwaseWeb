using Bunit;

using Heiwase.App.Blazor.Components.Sections.Home.AboutSection;
using Heiwase.App.Blazor.Components.Sections.Home.CoachesSection;
using Heiwase.App.Blazor.Components.Sections.Home.DojoSection;
using Heiwase.App.Blazor.Components.Sections.Home.MwkszSection;
using Heiwase.App.Blazor.Components.Sections.Home.WhyUsSection;
using Heiwase.App.Tests.Blazor.Support;

using Microsoft.AspNetCore.Components;

namespace Heiwase.App.Tests.Blazor.Sections;

/// <summary>
/// The "button opens a dialog" sections. Each button must open its own dialog component in the shared dialog frame,
/// in both language variants.
/// </summary>
public sealed class DialogSectionTests : SiteTestContext
{
    private static readonly string[] Cultures = [Hungarian, English];

    public static TheoryData<string> BothCultures => [.. Cultures];

    private DialogRecorder RenderAndClickAll<TSection>(string culture, string[] expectedDialogs, string buttonSelector = ".btn-info")
        where TSection : IComponent
    {
        UseCulture(culture);
        var dialogs = RecordDialogs();
        var cut = Render<TSection>();

        var buttons = cut.FindAll(buttonSelector);
        Assert.Equal(expectedDialogs.Length, buttons.Count);

        foreach (var button in buttons)
        {
            button.Click();
        }

        Assert.Equal(expectedDialogs, dialogs.Opened.Select(d => d.ComponentType.Name));
        Assert.All(dialogs.Opened, d =>
        {
            Assert.False(string.IsNullOrWhiteSpace(d.Title));
            Assert.Equal("app-dialog", d.Options.CssClass);
            Assert.True(d.Options.ShowClose);
        });

        return dialogs;
    }

    [Theory]
    [MemberData(nameof(BothCultures))]
    public void About_Buttons_Open_History_Mission_And_Medal_Dialogs(string culture)
        => RenderAndClickAll<AboutSection>(culture, ["ClubHistoryDialog", "StudentCountDialog", "MedalDialog"]);

    [Theory]
    [MemberData(nameof(BothCultures))]
    public void Coaches_Buttons_Open_One_Dialog_Per_Coach_With_Wide_Frame(string culture)
    {
        var dialogs = RenderAndClickAll<CoachesSection>(culture, ["CoachZoltanDialog", "CoachBendeguzDialog", "CoachBarnabasDialog"]);

        Assert.All(dialogs.Opened, d => Assert.Equal("min(720px, 92vw)", d.Options.Width));
    }

    [Theory]
    [MemberData(nameof(BothCultures))]
    public void Mwksz_Buttons_Open_Federation_Dialogs(string culture)
        => RenderAndClickAll<MwkszSection>(culture, ["BeltExamDialog", "TrainingCampDialog", "CompetitionsDialog", "SeminarsDialog"]);

    [Theory]
    [MemberData(nameof(BothCultures))]
    public void WhyUs_Buttons_Open_Training_Type_Dialogs(string culture)
        => RenderAndClickAll<WhyUsSection>(culture, ["WadoRyuDialog", "SportkarateDialog", "WomensSelfDefenseDialog", "AthleticsDialog"]);

    [Theory]
    [MemberData(nameof(BothCultures))]
    public void Dojo_Video_Button_Opens_Borderless_Video_Dialog(string culture)
    {
        var dialogs = RenderAndClickAll<DojoSection>(culture, ["DojoVideoDialog"], ".dojo-video-dialog button");

        Assert.Contains("app-dialog-content-flush", dialogs.OnlyDialog.Options.ContentCssClass, StringComparison.Ordinal);
    }

    [Theory]
    [MemberData(nameof(BothCultures))]
    public void Dojo_Pictures_Open_The_Clicked_Image(string culture)
    {
        UseCulture(culture);
        var dialogs = RecordDialogs();
        var cut = Render<DojoSection>();

        foreach (var picture in cut.FindAll("picture.gallery-img"))
        {
            picture.Click();
        }

        string[] images = [.. dialogs.Opened.Select(d => (string) d.Parameters!["ImagePath"]!)];
        Assert.Equal(4, images.Length);
        Assert.EndsWith("/kumite.jpg", images[0], StringComparison.Ordinal);
        Assert.EndsWith("/dojoedzes.jpg", images[1], StringComparison.Ordinal);
        Assert.EndsWith("/bejarat.jpg", images[2], StringComparison.Ordinal);
        Assert.EndsWith("/arnyek.jpg", images[3], StringComparison.Ordinal);
        Assert.All(dialogs.Opened, d =>
        {
            Assert.Equal("PictureShowDialog", d.ComponentType.Name);
            Assert.Equal("auto", d.Options.Width);
            Assert.False(string.IsNullOrEmpty((string?) d.Parameters!["AltText"]));
        });
    }

    [Fact]
    public void Sections_Are_Localised()
    {
        UseCulture(Hungarian);
        var hungarian = Render<AboutSection>().Markup;
        UseCulture(English);
        var english = Render<AboutSection>().Markup;

        Assert.Contains("óta Heiwase", hungarian, StringComparison.Ordinal);
        Assert.Contains("Heiwase Founded", english, StringComparison.Ordinal);
    }

    [Fact]
    public void Dialog_Titles_Follow_The_Current_Culture()
    {
        var dialogs = RecordDialogs();

        UseCulture(Hungarian);
        Render<AboutSection>().Find(".btn-info").Click();
        UseCulture(English);
        Render<AboutSection>().Find(".btn-info").Click();

        Assert.Equal(2, dialogs.Opened.Count);
        Assert.NotEqual(dialogs.Opened[0].Title, dialogs.Opened[1].Title);
    }
}
