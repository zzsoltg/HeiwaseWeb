using System.ComponentModel.DataAnnotations;
using System.Text.Json;

using Heiwase.App.Api.Mappers;
using Heiwase.App.Api.Services;
using Heiwase.App.Infrastructure.Interface;
using Heiwase.App.Shared.Models;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;

namespace Heiwase.App.Api;

public sealed class ApplicationFormFunction
{
    private readonly IApplicationFormRepository _repository;
    private readonly ITurnstileValidator _turnstileValidator;

    private readonly IApplicationNotifier _notifier;

    public ApplicationFormFunction(
        IApplicationFormRepository repository,
        ITurnstileValidator turnstileValidator,
        IApplicationNotifier notifier)
    {
        ArgumentNullException.ThrowIfNull(repository);
        ArgumentNullException.ThrowIfNull(turnstileValidator);
        ArgumentNullException.ThrowIfNull(notifier);
        _repository = repository;
        _turnstileValidator = turnstileValidator;
        _notifier = notifier;
    }

    // [Authorize(Roles = "Administrator")] -> This attribute is required for all admin POST methods in the API Endpoints.
    [Function("SubmitApplication")]
    public async Task<IActionResult> SubmitAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "applications")]
        HttpRequest req)
    {
        ArgumentNullException.ThrowIfNull(req);

        ApplicationFormDto? application;
        try
        {
            application = await req.ReadFromJsonAsync<ApplicationFormDto>(
                cancellationToken: req.HttpContext.RequestAborted).ConfigureAwait(false);
        }
        catch (JsonException)
        {
            return BadRequest("The request body must contain a valid application.");
        }

        if (application is null)
        {
            return BadRequest("The request body must contain an application.");
        }

        List<ValidationResult> validationResults = [];
        if (!Validator.TryValidateObject(application, new ValidationContext(application), validationResults, validateAllProperties: true))
        {
            var errors = validationResults
                .GroupBy(result => result.MemberNames.FirstOrDefault() ?? string.Empty)
                .ToDictionary(
                    group => group.Key,
                    group => group.Select(result => result.ErrorMessage ?? "Invalid value.").ToArray());

            return new BadRequestObjectResult(new ValidationProblemDetails(errors));
        }

        var cancellationToken = req.HttpContext.RequestAborted;

        bool captchaPassed;
        try
        {
            captchaPassed = await _turnstileValidator.ValidateAsync(application.CaptchaToken, cancellationToken).ConfigureAwait(false);
        }
        catch (HttpRequestException)
        {
            return new StatusCodeResult(StatusCodes.Status503ServiceUnavailable);
        }

        if (!captchaPassed)
        {
            return new BadRequestObjectResult(new ValidationProblemDetails(new Dictionary<string, string[]>
            {
                [nameof(ApplicationFormDto.CaptchaToken)] = ["The security check failed. Please try again."]
            }));
        }

        var entity = application.ToEntity();
        await _repository.AddAsync(entity, cancellationToken).ConfigureAwait(false);
        await _notifier.NotifyAsync(entity, CancellationToken.None).ConfigureAwait(false);

        return new OkResult();
    }

    private static BadRequestObjectResult BadRequest(string message)
        => new(new ValidationProblemDetails(new Dictionary<string, string[]>
        {
            ["application"] = [message]
        }));
}
