using System.Net;
using System.Text;
using System.Text.Json;

using Heiwase.App.Shared.Models;

namespace Heiwase.App.Tests.Blazor.Support;

public sealed record RecordedRequest(HttpMethod Method, Uri? Uri, string Body);

/// <summary>An <see cref="HttpMessageHandler"/> that answers with a fixed response and records what was sent.</summary>
public sealed class StubHttpHandler(Func<HttpRequestMessage, HttpResponseMessage> responder) : HttpMessageHandler
{
    public List<RecordedRequest> Requests { get; } = [];

    public static StubHttpHandler Status(HttpStatusCode status)
        => new(_ => new HttpResponseMessage(status));

    public static StubHttpHandler Json(object payload)
    {
        string json = JsonSerializer.Serialize(payload);
        return new StubHttpHandler(_ => new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json"),
        });
    }

    public HttpClient CreateClient()
        => new(this) { BaseAddress = new Uri("http://localhost/") };

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        string body = request.Content is null ? string.Empty : await request.Content.ReadAsStringAsync(cancellationToken);
        Requests.Add(new RecordedRequest(request.Method, request.RequestUri, body));
        return responder(request);
    }
}

public static class HallOfFameTestData
{
    public static HallOfFameMemberDto Member(string id, string name, string title = "Senior", params string[] results)
        => new()
        {
            Id = id,
            Names = new Dictionary<string, string> { ["hu"] = $"{name} HU", ["en"] = name },
            Titles = new Dictionary<string, string> { ["hu"] = $"{title} HU", ["en"] = title },
            Descriptions = new Dictionary<string, List<string>>
            {
                ["hu"] = [.. results.Select(r => $"{r} HU")],
                ["en"] = [.. results],
            },
            ImagePath = $"img/hall_of_fame/{id}.png",
        };

    public static HallOfFameDataDto Data(int competitors, int senpais)
        => new()
        {
            Competitors = [.. Enumerable.Range(1, competitors).Select(i => Member($"c{i}", $"Competitor {i}", "Kumite", $"Gold {i}"))],
            Senpais = [.. Enumerable.Range(1, senpais).Select(i => Member($"s{i}", $"Senpai {i}", i == 1 ? "1st dan" : "3rd kyu", $"Exam {i}"))],
        };
}
