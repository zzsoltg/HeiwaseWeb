using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Heiwase.App.Api;

public class SeedHallOfFameFunction
{
    private readonly HallOfFameSeeder _seeder;
    private readonly ILogger<SeedHallOfFameFunction> _logger;

    public SeedHallOfFameFunction(HallOfFameSeeder seeder, ILogger<SeedHallOfFameFunction> logger)
    {
        _seeder = seeder;
        _logger = logger;
    }

    [Function("SeedHallOfFame")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "seed/hall-of-fame")] HttpRequest req)
    {
        string dataDirectory = Path.Combine(AppContext.BaseDirectory, "data");
        HallOfFameSeedResult result = await _seeder.SeedAsync(dataDirectory, req.HttpContext.RequestAborted).ConfigureAwait(false);

        _logger.LogInformation("Hall of Fame seeded: {Created} created, {Updated} updated.", result.Created, result.Updated);
        return new OkObjectResult(result);
    }
}
