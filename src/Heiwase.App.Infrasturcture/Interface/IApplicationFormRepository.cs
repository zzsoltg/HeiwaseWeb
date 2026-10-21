using Heiwase.App.Infrastructure.Entities;

namespace Heiwase.App.Infrastructure.Interface;

public interface IApplicationFormRepository : IRepository<ApplicationFormEntity>
{
    /// <summary>Returns applications not yet reviewed, oldest first.</summary>
    Task<IReadOnlyList<ApplicationFormEntity>> GetPendingAsync(CancellationToken cancellationToken = default);
}
