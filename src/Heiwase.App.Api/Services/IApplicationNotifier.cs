using Heiwase.App.Infrastructure.Entities;

namespace Heiwase.App.Api.Services;

public interface IApplicationNotifier
{
    /// <summary>
    /// Notifies the club administrators about a stored application. Implementations must not throw:
    /// the application is already saved and a notification problem must not fail the submission.
    /// </summary>
    Task NotifyAsync(ApplicationFormEntity application, CancellationToken cancellationToken = default);
}
