using Heiwase.App.Shared.Models;

namespace Heiwase.App.Blazor.Services.Interface;

public interface IApplicationFormService
{
    /// <summary>
    /// Submits an application form to the API.
    /// </summary>
    Task<bool> SubmitAsync(ApplicationFormDto application, CancellationToken cancellationToken = default);
}
