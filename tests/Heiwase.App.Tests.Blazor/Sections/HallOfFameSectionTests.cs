using System.Reflection;

using Bunit;

using Heiwase.App.Blazor.Components.Sections.Home.HallOfFameSection;
using Heiwase.App.Shared.Models;
using Heiwase.App.Tests.Blazor.Support;

using Microsoft.Extensions.DependencyInjection;

namespace Heiwase.App.Tests.Blazor.Sections;

public sealed class HallOfFameSectionTests : SiteTestContext
{
    private const string Grid = "competitors-grid";

    private readonly BunitJSModuleInterop _module;

    public HallOfFameSectionTests()
    {
        _module = JSInterop.SetupModule("./js/animations.js");
        _module.Mode = JSRuntimeMode.Loose;
    }

    private IRenderedComponent<HallOfFameSection> RenderSection(int competitors = 6, int senpais = 6, StubHttpHandler? handler = null)
    {
        Services.AddSingleton((handler ?? StubHttpHandler.Json(HallOfFameTestData.Data(competitors, senpais))).CreateClient());
        return Render<HallOfFameSection>();
    }

    private static string[] CompetitorNames(IRenderedComponent<HallOfFameSection> cut)
        => [.. cut.Find("#competitors-grid").QuerySelectorAll(".card-name").Select(n => n.TextContent)];

    private static string[] SenpaiNames(IRenderedComponent<HallOfFameSection> cut)
        => [.. cut.Find("#senpais-grid").QuerySelectorAll(".card-name").Select(n => n.TextContent)];

    private static HallOfFameSection_en_US Section(IRenderedComponent<HallOfFameSection> cut)
        => cut.FindComponent<HallOfFameSection_en_US>().Instance;

    private static object? Invoke(object instance, string method, params object?[] args)
        => typeof(HallOfFameSection).GetMethod(method, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)!.Invoke(instance, args);

    private static T Get<T>(object instance, string property)
        => (T) typeof(HallOfFameSection).GetProperty(property, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)!.GetValue(instance)!;

    [Fact]
    public void Shows_A_Loading_Message_When_The_Data_Cannot_Be_Loaded()
    {
        var cut = RenderSection(handler: StubHttpHandler.Status(System.Net.HttpStatusCode.NotFound));

        Assert.Equal("Loading data...", cut.Find(".text-center").TextContent);
        Assert.Empty(cut.FindAll(".card-common"));
    }

    [Fact]
    public void Requests_The_Hall_Of_Fame_Json()
    {
        StubHttpHandler handler = StubHttpHandler.Json(HallOfFameTestData.Data(6, 6));

        RenderSection(handler: handler);

        Assert.NotEmpty(handler.Requests);
        Assert.All(handler.Requests, r => Assert.Equal("http://localhost/data/halloffame.json", r.Uri?.ToString()));
    }

    [Fact]
    public void Renders_A_Window_Of_Five_Cards_Per_Track_Starting_With_The_Last_Item()
    {
        var cut = RenderSection();

        Assert.Equal(["Competitor 6", "Competitor 1", "Competitor 2", "Competitor 3", "Competitor 4"], CompetitorNames(cut));
        Assert.Equal(["Senpai 6", "Senpai 1", "Senpai 2", "Senpai 3", "Senpai 4"], SenpaiNames(cut));
    }

    [Fact]
    public void Small_Data_Sets_Wrap_Around()
    {
        var cut = RenderSection(competitors: 2, senpais: 1);

        Assert.Equal(["Competitor 2", "Competitor 1", "Competitor 2", "Competitor 1", "Competitor 2"], CompetitorNames(cut));
        Assert.Equal(5, SenpaiNames(cut).Length);
    }

    [Fact]
    public void Senpai_Track_Is_Hidden_Without_Senpais()
    {
        var cut = RenderSection(competitors: 3, senpais: 0);

        Assert.NotEmpty(cut.FindAll("#competitors-grid"));
        Assert.Empty(cut.FindAll("#senpais-grid"));
    }

    [Fact]
    public void Names_Follow_The_Current_Culture()
    {
        UseCulture(Hungarian);

        var cut = RenderSection();

        Assert.All(CompetitorNames(cut), n => Assert.EndsWith(" HU", n, StringComparison.Ordinal));
    }

    [Fact]
    public void Photo_Sources_Use_Modern_Image_Formats_With_A_Fallback()
    {
        var cut = RenderSection();

        var card = cut.Find("#competitors-grid .card-common");
        Assert.EndsWith("Competitor 6", card.QuerySelector("img")!.GetAttribute("alt"), StringComparison.Ordinal);
        Assert.EndsWith(".avif", card.QuerySelector("source[type='image/avif']")!.GetAttribute("srcset"), StringComparison.Ordinal);
        Assert.EndsWith(".webp", card.QuerySelector("source[type='image/webp']")!.GetAttribute("srcset"), StringComparison.Ordinal);
        Assert.EndsWith(".jpg", card.QuerySelector("img")!.GetAttribute("src"), StringComparison.Ordinal);
    }

    [Fact]
    public void Initialises_The_Animation_Module_And_Both_Tracks_After_The_First_Render()
    {
        RenderSection();

        Assert.NotEmpty(_module.Invocations["initAnimations"]);
        Assert.True(_module.Invocations["initTrack"].Count >= 2);
    }

    [Fact]
    public void Does_Not_Initialise_Tracks_Without_Data()
    {
        RenderSection(handler: StubHttpHandler.Status(System.Net.HttpStatusCode.InternalServerError));

        _module.VerifyNotInvoke("initTrack");
    }

    [Fact]
    public void Next_Button_Shows_The_Following_Competitor_After_Sliding_Left()
    {
        var cut = RenderSection();

        cut.FindAll(".slider-nav-btn")[1].Click();

        cut.WaitForAssertion(() => Assert.Equal("Competitor 1", CompetitorNames(cut)[0]));
        _module.VerifyInvoke("slideTrackLeft");
        cut.WaitForAssertion(() => _module.VerifyInvoke("resetTrack", calledTimes: 2));
    }

    [Fact]
    public void Previous_Button_Shows_The_Preceding_Competitor_After_Sliding_Right()
    {
        var cut = RenderSection();

        cut.FindAll(".slider-nav-btn")[0].Click();

        cut.WaitForAssertion(() => Assert.Equal("Competitor 5", CompetitorNames(cut)[0]));
        _module.VerifyInvoke("slideTrackRight");
    }

    [Fact]
    public void Senpai_Buttons_Move_The_Senpai_Track()
    {
        var cut = RenderSection();

        cut.FindAll(".slider-nav-btn")[3].Click();
        cut.WaitForAssertion(() => Assert.Equal("Senpai 1", SenpaiNames(cut)[0]));

        cut.FindAll(".slider-nav-btn")[2].Click();
        cut.WaitForAssertion(() => Assert.Equal("Senpai 6", SenpaiNames(cut)[0]));
        Assert.Equal("Competitor 6", CompetitorNames(cut)[0]);
    }

    [Fact]
    public void A_Click_During_A_Running_Animation_Is_Ignored()
    {
        var slide = _module.Setup<object>("slideTrackLeft", Grid);
        var cut = RenderSection();

        cut.FindAll(".slider-nav-btn")[1].Click();
        cut.FindAll(".slider-nav-btn")[1].Click();
        slide.SetResult(new object());

        cut.WaitForAssertion(() => Assert.Equal("Competitor 1", CompetitorNames(cut)[0]));
        _module.VerifyInvoke("slideTrackLeft", calledTimes: 1);
    }

    [Fact]
    public void Wraps_Around_When_Moving_Past_Either_End()
    {
        var cut = RenderSection(competitors: 3, senpais: 3);
        object section = Section(cut);

        Invoke(section, "NextCompetitor");
        Invoke(section, "NextCompetitor");
        Invoke(section, "NextCompetitor");
        Assert.Equal(0, Get<int>(section, "CompetitorIndex"));

        Invoke(section, "PrevCompetitor");
        Assert.Equal(2, Get<int>(section, "CompetitorIndex"));

        Invoke(section, "PrevSenpai");
        Assert.Equal(2, Get<int>(section, "SenpaiIndex"));
        Invoke(section, "NextSenpai");
        Assert.Equal(0, Get<int>(section, "SenpaiIndex"));
    }

    [Fact]
    public void Navigation_Without_Data_Does_Nothing()
    {
        var cut = RenderSection(handler: StubHttpHandler.Status(System.Net.HttpStatusCode.NotFound));
        object section = Section(cut);

        Invoke(section, "NextCompetitor");
        Invoke(section, "PrevCompetitor");
        Invoke(section, "NextSenpai");
        Invoke(section, "PrevSenpai");

        Assert.Equal(0, Get<int>(section, "CompetitorIndex"));
        Assert.Equal(0, Get<int>(section, "SenpaiIndex"));
        Assert.Empty((List<HallOfFameMemberDto>) Invoke(section, "GetCompetitorItems")!);
        Assert.Empty((List<HallOfFameMemberDto>) Invoke(section, "GetSenpaiItems")!);
    }

    [Fact]
    public void The_Auto_Slide_Timer_Advances_Both_Tracks_In_Opposite_Directions()
    {
        var cut = RenderSection();

        Invoke(Section(cut), "OnTimerElapsed", null, null);

        cut.WaitForAssertion(() => Assert.Equal("Competitor 5", CompetitorNames(cut)[0]));
        Assert.Equal("Senpai 1", SenpaiNames(cut)[0]);
        _module.VerifyInvoke("slideTrackRight");
        _module.VerifyInvoke("slideTrackLeft");
    }

    [Fact]
    public void A_Manual_Click_Pauses_The_Auto_Slide_Until_The_Resume_Timer_Elapses()
    {
        var cut = RenderSection();
        object section = Section(cut);

        cut.FindAll(".slider-nav-btn")[1].Click();
        Assert.True(Get<bool>(section, "UserInteractionPaused"));
        Assert.NotNull(typeof(HallOfFameSection).GetProperty("ResumeTimer", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(section));

        cut.WaitForAssertion(() => Assert.False(Get<bool>(section, "IsAnimating")));
        Invoke(section, "OnResumeTimerElapsed", null, null);

        Assert.False(Get<bool>(section, "UserInteractionPaused"));
    }

    [Fact]
    public void Competitor_Button_Opens_The_Results_Dialog_For_That_Member()
    {
        var cut = RenderSection();
        var dialogs = RecordDialogs();

        cut.Find("#competitors-grid .btn-info").Click();

        var dialog = dialogs.OnlyDialog;
        Assert.Equal("CompetitorResultsDialog", dialog.ComponentType.Name);
        Assert.StartsWith("Competitor 6", dialog.Title, StringComparison.Ordinal);
        Assert.Equal("c6", ((HallOfFameMemberDto) dialog.Parameters!["Member"]!).Id);
    }

    [Fact]
    public void Senpai_Button_Opens_The_Senpai_Dialog_For_That_Member()
    {
        var cut = RenderSection();
        var dialogs = RecordDialogs();

        cut.Find("#senpais-grid .btn-info").Click();

        var dialog = dialogs.OnlyDialog;
        Assert.Equal("SenpaiResultsDialog", dialog.ComponentType.Name);
        Assert.Equal("s6", ((HallOfFameMemberDto) dialog.Parameters!["Member"]!).Id);
    }

    [Fact]
    public async Task Disposing_Releases_The_Timers_And_The_Module()
    {
        var cut = RenderSection();
        object section = Section(cut);
        cut.FindAll(".slider-nav-btn")[1].Click();

        await ((IAsyncDisposable) section).DisposeAsync();

        Assert.NotNull(section);
    }
}
