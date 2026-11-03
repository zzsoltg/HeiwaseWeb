using Heiwase.App.Api.Mappers;
using Heiwase.App.Infrastructure.Interface;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;

namespace Heiwase.App.Api;

public class HallOfFameFunction
{
    private readonly IHallOfFameMemberRepository _repository;

    public HallOfFameFunction(IHallOfFameMemberRepository repository)
    {
        _repository = repository;
    }

    [Function("GetHallOfFame")]
    public async Task<IActionResult> GetAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "halloffame")]
        HttpRequest req)
    {
        ArgumentNullException.ThrowIfNull(req);

        var members = await _repository.GetOrderedAsync(req.HttpContext.RequestAborted).ConfigureAwait(false);

        return new OkObjectResult(members.ToDataDto());
    }
}
