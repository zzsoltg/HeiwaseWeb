using System.Net.Http.Json;

using Heiwase.App.Blazor.Services.Interface;
using Heiwase.App.Shared.Models;

namespace Heiwase.App.Blazor.Services.Implementation;

public sealed class HallOfFameService : IHallOfFameService
{
    public const string Endpoint = "api/halloffame";

    private readonly HttpClient _http;

    public HallOfFameService(HttpClient http)
    {
        _http = http;
    }

    public Task<HallOfFameDataDto?> GetAsync(CancellationToken cancellationToken = default)
        => _http.GetFromJsonAsync<HallOfFameDataDto>(Endpoint, cancellationToken);
}
