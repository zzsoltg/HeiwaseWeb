using System.Net;

using Bunit;

using Heiwase.App.Blazor.Components.Sections.Home.ContactSection;
using Heiwase.App.Shared.Enums;
using Heiwase.App.Tests.Blazor.Support;

using Microsoft.Extensions.DependencyInjection;

namespace Heiwase.App.Tests.Blazor.Sections;

public sealed class ContactSectionTests : SiteTestContext
{
    private const int Child = 0;
    private const int Adult = 1;
    private const int Sportkarate = 2;
    private const int WomenSelfDefense = 3;
    private const int Athletics = 4;

    private readonly StubHttpHandler _http = StubHttpHandler.Status(HttpStatusCode.OK);

    private IRenderedComponent<ContactSection> RenderForm(StubHttpHandler? handler = null)
    {
        Services.AddSingleton((handler ?? _http).CreateClient());
        return Render<ContactSection>();
    }

    private static AngleSharp.Dom.IElement Checkbox(IRenderedComponent<ContactSection> cut, int index)
        => cut.FindAll(".training-types-grid input[type=checkbox]")[index];

    private static bool IsDisabled(IRenderedComponent<ContactSection> cut, int index)
        => Checkbox(cut, index).HasAttribute("disabled");

    private static bool IsChecked(IRenderedComponent<ContactSection> cut, int index)
        => Checkbox(cut, index).HasAttribute("checked");

    private static void Fill(IRenderedComponent<ContactSection> cut, string name = "Teszt Elek", string email = "elek@example.com")
    {
        cut.Find("input[placeholder='Full Name']").Change(name);
        cut.Find("input[placeholder='example@email.com']").Change(email);
    }

    [Theory]
    [InlineData(Hungarian)]
    [InlineData(English)]
    public void Renders_Every_Training_Option_With_A_Localised_Label(string culture)
    {
        UseCulture(culture);

        IRenderedComponent<ContactSection> cut = RenderForm();

        string[] labels = [.. cut.FindAll(".training-type-option").Select(l => l.TextContent.Trim())];
        Assert.Equal(5, labels.Length);
        Assert.All(labels, l => Assert.False(string.IsNullOrWhiteSpace(l)));
        Assert.Equal(5, labels.Distinct().Count());
    }

    [Fact]
    public void English_Labels_Differ_From_Hungarian_Ones()
    {
        UseCulture(Hungarian);
        string[] hungarian = [.. RenderForm().FindAll(".training-type-option").Select(l => l.TextContent.Trim())];
        UseCulture(English);
        string[] english = [.. Render<ContactSection>().FindAll(".training-type-option").Select(l => l.TextContent.Trim())];

        Assert.NotEqual(hungarian, english);
    }

    [Fact]
    public void Nothing_Is_Disabled_Or_Checked_Initially()
    {
        IRenderedComponent<ContactSection> cut = RenderForm();

        Assert.All(Enumerable.Range(0, 5), i => Assert.False(IsDisabled(cut, i)));
        Assert.All(Enumerable.Range(0, 5), i => Assert.False(IsChecked(cut, i)));
        Assert.Empty(cut.FindAll(".form-field-visible"));
    }

    [Fact]
    public void Selecting_A_Training_Type_Checks_It_And_Unselecting_Clears_It()
    {
        IRenderedComponent<ContactSection> cut = RenderForm();

        Checkbox(cut, Sportkarate).Change(true);
        Assert.True(IsChecked(cut, Sportkarate));

        Checkbox(cut, Sportkarate).Change(false);
        Assert.False(IsChecked(cut, Sportkarate));
    }

    [Fact]
    public void Child_And_Adult_Training_Exclude_Each_Other()
    {
        IRenderedComponent<ContactSection> cut = RenderForm();

        Checkbox(cut, Adult).Change(true);
        Assert.True(IsDisabled(cut, Child));
        Assert.False(IsDisabled(cut, Adult));

        Checkbox(cut, Adult).Change(false);
        Checkbox(cut, Child).Change(true);
        Assert.True(IsDisabled(cut, Adult));
        Assert.False(IsDisabled(cut, Child));
    }

    [Fact]
    public void Women_Self_Defense_Is_Disabled_For_Men_Only()
    {
        IRenderedComponent<ContactSection> cut = RenderForm();

        cut.Find("select").Change(nameof(GenderType.Male));
        Assert.True(IsDisabled(cut, WomenSelfDefense));

        cut.Find("select").Change(nameof(GenderType.Female));
        Assert.False(IsDisabled(cut, WomenSelfDefense));
    }

    [Fact]
    public void Switching_To_Male_Drops_An_Already_Selected_Women_Self_Defense()
    {
        IRenderedComponent<ContactSection> cut = RenderForm();
        cut.Find("select").Change(nameof(GenderType.Female));
        Checkbox(cut, WomenSelfDefense).Change(true);
        Assert.True(IsChecked(cut, WomenSelfDefense));

        cut.Find("select").Change(nameof(GenderType.Male));

        Assert.False(IsChecked(cut, WomenSelfDefense));
    }

    [Fact]
    public void An_Adult_Birth_Date_Disables_And_Removes_Child_Training()
    {
        IRenderedComponent<ContactSection> cut = RenderForm();
        Checkbox(cut, Child).Change(true);

        cut.Find("input[type=date]").Change("1990-05-17");

        Assert.True(IsDisabled(cut, Child));
        Assert.False(IsChecked(cut, Child));
        Assert.Empty(cut.FindAll(".form-field-visible"));
    }

    [Fact]
    public void A_Minor_Birth_Date_Reveals_The_Guardian_Field_And_Keeps_Child_Training_Available()
    {
        IRenderedComponent<ContactSection> cut = RenderForm();
        string childBirthDate = DateTime.Today.AddYears(-10).ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);

        cut.Find("input[type=date]").Change(childBirthDate);

        Assert.Single(cut.FindAll(".form-field-visible"));
        Assert.False(IsDisabled(cut, Child));
    }

    [Fact]
    public void Athletics_Is_Never_Restricted()
    {
        IRenderedComponent<ContactSection> cut = RenderForm();
        cut.Find("select").Change(nameof(GenderType.Male));
        cut.Find("input[type=date]").Change("1990-05-17");
        Checkbox(cut, Adult).Change(true);

        Assert.False(IsDisabled(cut, Athletics));
        Assert.False(IsDisabled(cut, Sportkarate));
    }

    [Fact]
    public void Submitting_An_Empty_Form_Shows_Validation_Errors_And_Sends_Nothing()
    {
        IRenderedComponent<ContactSection> cut = RenderForm();

        cut.Find("form").Submit();

        Assert.NotEmpty(cut.FindAll(".form-validation-message"));
        Assert.Empty(_http.Requests);
    }

    [Fact]
    public void Submitting_An_Invalid_Email_Shows_An_Error_And_Sends_Nothing()
    {
        IRenderedComponent<ContactSection> cut = RenderForm();
        Fill(cut, email: "not-an-email");

        cut.Find("form").Submit();

        Assert.NotEmpty(cut.FindAll(".form-validation-message"));
        Assert.Empty(_http.Requests);
    }

    [Fact]
    public void A_Valid_Application_Is_Posted_To_Formspree_And_The_Form_Is_Reset()
    {
        IRenderedComponent<ContactSection> cut = RenderForm();
        Fill(cut);
        cut.Find("input[placeholder='+36...']").Change("+36 20 123 4567");
        cut.Find("select").Change(nameof(GenderType.Male));
        cut.Find("input[type=date]").Change("1990-05-17");
        Checkbox(cut, Adult).Change(true);
        Checkbox(cut, Athletics).Change(true);

        cut.Find("form").Submit();

        cut.WaitForAssertion(() => Assert.Single(cut.FindAll(".form-success-message.form-message-visible")));
        RecordedRequest request = Assert.Single(_http.Requests);
        Assert.Equal(HttpMethod.Post, request.Method);
        Assert.Equal("https://formspree.io/f/mrenpyzo", request.Uri?.ToString());
        Assert.Contains("\"name\":\"Teszt Elek\"", request.Body, StringComparison.Ordinal);
        Assert.Contains("\"email\":\"elek@example.com\"", request.Body, StringComparison.Ordinal);
        Assert.Contains("\"phone\":\"\\u002B36 20 123 4567\"", request.Body, StringComparison.Ordinal);
        Assert.Contains("\"dateOfBirth\":\"1990-05-17\"", request.Body, StringComparison.Ordinal);
        Assert.Contains("Adult, Athletics", request.Body, StringComparison.Ordinal);
        Assert.Empty(cut.FindAll(".form-error-message.form-message-visible"));
        Assert.Equal(string.Empty, cut.Find("input[placeholder='Full Name']").GetAttribute("value") ?? string.Empty);
        Assert.False(IsChecked(cut, Adult));
    }

    [Fact]
    public void A_Failed_Submission_Shows_The_Error_Message_And_Keeps_The_Input()
    {
        StubHttpHandler failing = StubHttpHandler.Status(HttpStatusCode.InternalServerError);
        IRenderedComponent<ContactSection> cut = RenderForm(failing);
        Fill(cut);

        cut.Find("form").Submit();

        cut.WaitForAssertion(() => Assert.Single(cut.FindAll(".form-error-message.form-message-visible")));
        Assert.Empty(cut.FindAll(".form-success-message.form-message-visible"));
        Assert.Equal("Teszt Elek", cut.Find("input[placeholder='Full Name']").GetAttribute("value"));
    }

    [Fact]
    public void A_Minor_Application_Requires_A_Guardian()
    {
        IRenderedComponent<ContactSection> cut = RenderForm();
        Fill(cut);
        cut.Find("input[type=date]").Change(DateTime.Today.AddYears(-10).ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture));

        cut.Find("form").Submit();

        Assert.NotEmpty(cut.FindAll(".form-validation-message"));
        Assert.Empty(_http.Requests);
    }
}
