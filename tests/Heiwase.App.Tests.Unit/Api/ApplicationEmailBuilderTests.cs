using Heiwase.App.Api.Services;
using Heiwase.App.Infrastructure.Entities;

namespace Heiwase.App.Tests.Unit.Api;

public sealed class ApplicationEmailBuilderTests
{
    [Fact]
    public void Html_Encodes_User_Input()
    {
        var html = ApplicationEmailBuilder.BuildHtml(new ApplicationFormEntity { Name = "<script>x</script>" });

        Assert.DoesNotContain("<script>", html, StringComparison.Ordinal);
        Assert.Contains("&lt;script&gt;", html, StringComparison.Ordinal);
    }

    [Fact]
    public void Subject_Strips_Control_Characters()
    {
        var subject = ApplicationEmailBuilder.BuildSubject(new ApplicationFormEntity { Name = "Jane\r\nBcc: x" });

        Assert.DoesNotContain('\r', subject);
        Assert.DoesNotContain('\n', subject);
    }
}
