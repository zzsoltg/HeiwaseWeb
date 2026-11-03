using Heiwase.App.Shared.Models;

namespace Heiwase.App.Blazor.Services;

public interface IHallOfFameService
{
    /// <summary>
    /// Loads the Hall of Fame members from the API.
    /// </summary>
    Task<HallOfFameDataDto?> GetAsync(CancellationToken cancellationToken = default);
}
