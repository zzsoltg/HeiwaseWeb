using System.Reflection;

using Bunit;

using Heiwase.App.Blazor.Components.Shared.MwkszGallery;
using Heiwase.App.Tests.Blazor.Support;

namespace Heiwase.App.Tests.Blazor.Shared;

public sealed class MwkszGallerySliderTests : SiteTestContext
{
    private static readonly TimeSpan SlideTimeout = TimeSpan.FromSeconds(5);

    private static string[] FirstImages(IRenderedComponent<MwkszGallerySlider> cut)
        => [.. cut.FindAll(".gallery-collage").Select(c => c.QuerySelector("img")!.GetAttribute("src")!.Split('/')[^1])];

    private static void WaitForSlideEnd(IRenderedComponent<MwkszGallerySlider> cut)
        => cut.WaitForAssertion(() => Assert.Single(cut.FindAll(".gallery-collage")), SlideTimeout);

    [Fact]
    public void Shows_The_First_Page_Of_Four_Pictures()
    {
        IRenderedComponent<MwkszGallerySlider> cut = Render<MwkszGallerySlider>();

        Assert.Equal(4, cut.FindAll("picture").Count);
        Assert.Equal(["csoport.jpg"], FirstImages(cut));
    }

    [Fact]
    public void Uses_The_Supplied_Accessibility_Texts()
    {
        IRenderedComponent<MwkszGallerySlider> cut = Render<MwkszGallerySlider>(p => p
            .Add(c => c.ImageAltText, "Fotó")
            .Add(c => c.PreviousButtonAriaLabel, "Előző")
            .Add(c => c.NextButtonAriaLabel, "Következő"));

        AngleSharp.Dom.IElement[] buttons = [.. cut.FindAll(".gallery-nav-btn")];
        Assert.Equal("Előző", buttons[0].GetAttribute("aria-label"));
        Assert.Equal("Következő", buttons[1].GetAttribute("aria-label"));
        Assert.All(cut.FindAll("img"), i => Assert.Equal("Fotó", i.GetAttribute("alt")));
    }

    [Fact]
    public void Next_Slides_To_The_Second_Page_With_Enter_And_Exit_Animations()
    {
        IRenderedComponent<MwkszGallerySlider> cut = Render<MwkszGallerySlider>();

        cut.FindAll(".gallery-nav-btn")[1].Click();

        Assert.Equal(2, cut.FindAll(".gallery-collage").Count);
        Assert.Single(cut.FindAll(".gallery-slide-out-left"));
        Assert.Single(cut.FindAll(".gallery-slide-in-right"));
        WaitForSlideEnd(cut);
        Assert.Equal(["biro.jpg"], FirstImages(cut));
        Assert.Empty(cut.FindAll(".gallery-slide-in-right"));
    }

    [Fact]
    public void Previous_Wraps_From_The_First_To_The_Last_Page()
    {
        IRenderedComponent<MwkszGallerySlider> cut = Render<MwkszGallerySlider>();

        cut.FindAll(".gallery-nav-btn")[0].Click();

        Assert.Single(cut.FindAll(".gallery-slide-out-right"));
        Assert.Single(cut.FindAll(".gallery-slide-in-left"));
        WaitForSlideEnd(cut);
        Assert.Equal(["sem_eletkep.jpg"], FirstImages(cut));
    }

    [Fact]
    public void Next_Wraps_From_The_Last_Page_Back_To_The_First()
    {
        IRenderedComponent<MwkszGallerySlider> cut = Render<MwkszGallerySlider>();

        for (int i = 0; i < 3; i++)
        {
            cut.FindAll(".gallery-nav-btn")[1].Click();
            WaitForSlideEnd(cut);
        }

        Assert.Equal(["csoport.jpg"], FirstImages(cut));
    }

    [Fact]
    public void Clicks_While_Animating_Are_Ignored()
    {
        IRenderedComponent<MwkszGallerySlider> cut = Render<MwkszGallerySlider>();

        cut.FindAll(".gallery-nav-btn")[1].Click();
        cut.FindAll(".gallery-nav-btn")[1].Click();
        WaitForSlideEnd(cut);

        Assert.Equal(["biro.jpg"], FirstImages(cut));
    }

    [Fact]
    public void The_Auto_Slide_Timer_Advances_To_The_Next_Page()
    {
        IRenderedComponent<MwkszGallerySlider> cut = Render<MwkszGallerySlider>();

        typeof(MwkszGallerySlider)
            .GetMethod("OnAutoSlideElapsed", BindingFlags.Instance | BindingFlags.NonPublic)!
            .Invoke(cut.Instance, [null, null]);

        cut.WaitForAssertion(() => Assert.Equal(["biro.jpg"], FirstImages(cut)), SlideTimeout);
    }

    [Fact]
    public void Disposing_Stops_The_Auto_Slide_Timer()
    {
        IRenderedComponent<MwkszGallerySlider> cut = Render<MwkszGallerySlider>();
        System.Timers.Timer timer = (System.Timers.Timer)typeof(MwkszGallerySlider)
            .GetField("_autoSlideTimer", BindingFlags.Instance | BindingFlags.NonPublic)!
            .GetValue(cut.Instance)!;
        Assert.True(timer.Enabled);

        cut.Instance.Dispose();

        Assert.False(timer.Enabled);
    }
}
