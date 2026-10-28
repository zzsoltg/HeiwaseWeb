using Bunit;
using Bunit.TestDoubles;

using Heiwase.App.Blazor.Components.Layout;
using Heiwase.App.Tests.Blazor.Support;

using Microsoft.Extensions.DependencyInjection;

namespace Heiwase.App.Tests.Blazor.Layout;

public sealed class LanguageSelectorTests : SiteTestContext
{
    [Theory]
    [InlineData(Hungarian, "eng-lang-ico")]
    [InlineData(English, "hun-lang-ico")]
    public void Shows_The_Flag_Of_The_Other_Language(string culture, string expectedIcon)
    {
        UseCulture(culture);

        var cut = Render<LanguageSelector>();

        Assert.Contains(expectedIcon, cut.Find("img").GetAttribute("src"), StringComparison.Ordinal);
        Assert.Contains(expectedIcon, cut.Find("source[type='image/avif']").GetAttribute("srcset"), StringComparison.Ordinal);
        Assert.Contains(expectedIcon, cut.Find("source[type='image/webp']").GetAttribute("srcset"), StringComparison.Ordinal);
    }

    [Theory]
    [InlineData(Hungarian, "en-US")]
    [InlineData(English, "hu-HU")]
    public void Click_Stores_The_New_Culture_And_Reloads_The_Page(string culture, string expectedCulture)
    {
        UseCulture(culture);
        var cut = Render<LanguageSelector>();

        cut.Find(".language-selector-alignment").Click();

        var call = Assert.Single(JSInterop.Invocations["localStorage.setItem"]);
        Assert.Equal(["blazorCulture", expectedCulture], call.Arguments.Cast<string>());

        var navigation = Services.GetRequiredService<BunitNavigationManager>();
        var reload = Assert.Single(navigation.History);
        Assert.True(reload.Options.ForceLoad);
    }
}
