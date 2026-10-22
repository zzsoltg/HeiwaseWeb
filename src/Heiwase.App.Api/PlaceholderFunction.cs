using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Heiwase.App.Api;

public class PlaceholderFunction
{
    private readonly ILogger<PlaceholderFunction> _logger;

    public PlaceholderFunction(ILogger<PlaceholderFunction> logger)
    {
        _logger = logger;
    }

    [Function("Function")]
    public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");

        string? name = req.Query[ "name" ];
        return new OkObjectResult(string.IsNullOrWhiteSpace(name)
            ? "Welcome to Azure Functions!"
            : $"Welcome to Azure Functions! Hello, {name}");
    }
}