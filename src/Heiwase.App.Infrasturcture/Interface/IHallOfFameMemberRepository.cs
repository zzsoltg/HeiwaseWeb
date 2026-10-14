using Heiwase.App.Infrastructure.Entities;

namespace Heiwase.App.Infrastructure.Interface;

public interface IHallOfFameMemberRepository : IRepository<HallOfFameMemberEntity>
{
    /// <summary>Returns all members ordered by their display order.</summary>
    Task<IReadOnlyList<HallOfFameMemberEntity>> GetOrderedAsync(CancellationToken cancellationToken = default);
}
