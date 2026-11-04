using System.Net.Http.Json;

using Heiwase.App.Blazor.Services.Interface;
using Heiwase.App.Shared.Models;

namespace Heiwase.App.Blazor.Services.Implementation;

public class ApplicationFormService : IApplicationFormService
{
    public const string Endpoint = "api/applications";

    private readonly HttpClient _http;

    public ApplicationFormService(HttpClient http)
    {
        _http = http;
    }
    public async Task<bool> SubmitAsync(ApplicationFormDto application, CancellationToken cancellationToken = default)
    {
        var response = await _http.PostAsJsonAsync(Endpoint, application, cancellationToken);
        return response.IsSuccessStatusCode;
    }   
}
