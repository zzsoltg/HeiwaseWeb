using System.Globalization;
using System.Net;
using System.Text;

using Heiwase.App.Infrastructure.Entities;

namespace Heiwase.App.Api.Services;

public static class ApplicationEmailBuilder
{
    public static string BuildSubject(ApplicationFormEntity application)
    {
        ArgumentNullException.ThrowIfNull(application);
        return $"New application: {StripControlCharacters(application.Name)}";
    }

    public static string BuildPlainText(ApplicationFormEntity application)
    {
        ArgumentNullException.ThrowIfNull(application);

        StringBuilder text = new();
        foreach (var (label, value) in GetFields(application))
        {
            text.Append(label).Append(": ").AppendLine(value);
        }

        return text.ToString();
    }

    public static string BuildHtml(ApplicationFormEntity application)
    {
        ArgumentNullException.ThrowIfNull(application);

        StringBuilder html = new("<h2>New application</h2><table cellpadding=\"4\">");
        foreach (var (label, value) in GetFields(application))
        {
            html.Append("<tr><td><b>")
                .Append(WebUtility.HtmlEncode(label))
                .Append("</b></td><td>")
                .Append(WebUtility.HtmlEncode(value).ReplaceLineEndings("<br />"))
                .Append("</td></tr>");
        }

        return html.Append("</table>").ToString();
    }

    private static IEnumerable<(string Label, string Value)> GetFields(ApplicationFormEntity application)
    {
        yield return ("Name", application.Name);
        yield return ("Email", application.Email);
        yield return ("Phone", application.Phone);
        yield return ("Sex", application.Sex);
        yield return ("Date of birth", application.DateOfBirth?.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) ?? string.Empty);
        yield return ("Guardian", application.GuardianName);
        yield return ("Training types", string.Join(", ", application.TrainingTypes));
        yield return ("Message", application.Message);
        yield return ("Submitted (UTC)", application.SubmittedAt.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture));
    }

    private static string StripControlCharacters(string value)
        => new([.. value.Where(character => !char.IsControl(character))]);
}
