using Bunit;

using Heiwase.App.Blazor.Components.Layout;
using Heiwase.App.Tests.Blazor.Support;

namespace Heiwase.App.Tests.Blazor.Layout;

public sealed class NavigationMenuTests : SiteTestContext
{
    [Fact]
    public void Renders_Hungarian_Labels()
    {
        UseCulture(Hungarian);

        var cut = Render<NavigationMenu>();

        string[] labels = [.. cut.FindAll(".nav-links a").Select(a => a.TextContent.Trim())];
        Assert.Contains("Kezdőlap", labels);
        Assert.Contains("Kapcsolat", labels);
        Assert.Contains("Dojo", labels);
    }

    [Fact]
    public void Renders_English_Labels()
    {
        UseCulture(English);

        var cut = Render<NavigationMenu>();

        string[] labels = [.. cut.FindAll(".nav-links a").Select(a => a.TextContent.Trim())];
        Assert.DoesNotContain("Kezdőlap", labels);
        Assert.Equal(9, labels.Length);
    }

    [Fact]
    public void Links_Point_To_Every_Page_Section()
    {
        var cut = Render<NavigationMenu>();

        string[] targets = [.. cut.FindAll(".nav-links a").Select(a => a.GetAttribute("href")!)];

        Assert.Equal(
            ["#home", "#why-us", "#about", "#coaches", "#mwksz", "#halloffame", "#dojo", "#timetable", "#contact"],
            targets);
    }

    [Fact]
    public void Embeds_The_Language_Selector()
        => Assert.NotNull(Render<NavigationMenu>().FindComponent<LanguageSelector>());

    [Fact]
    public void Menu_Starts_Closed()
    {
        var cut = Render<NavigationMenu>();

        Assert.DoesNotContain("active", cut.Find(".hamburger").ClassList);
        Assert.DoesNotContain("active", cut.Find(".nav-links").ClassList);
    }

    [Fact]
    public void Hamburger_Toggles_The_Menu()
    {
        var cut = Render<NavigationMenu>();

        cut.Find(".hamburger").Click();

        Assert.Contains("active", cut.Find(".hamburger").ClassList);
        Assert.Contains("active", cut.Find(".nav-links").ClassList);

        cut.Find(".hamburger").Click();

        Assert.DoesNotContain("active", cut.Find(".nav-links").ClassList);
    }

    [Fact]
    public void Clicking_A_Link_Closes_The_Menu()
    {
        var cut = Render<NavigationMenu>();
        cut.Find(".hamburger").Click();

        cut.Find(".nav-links a[href='#about']").Click();

        Assert.DoesNotContain("active", cut.Find(".nav-links").ClassList);
        Assert.DoesNotContain("active", cut.Find(".hamburger").ClassList);
    }
}
