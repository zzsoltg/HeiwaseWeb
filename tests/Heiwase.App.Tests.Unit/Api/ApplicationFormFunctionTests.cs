using System.Text.Json;

using Heiwase.App.Api;
using Heiwase.App.Api.Services;
using Heiwase.App.Infrastructure.Entities;
using Heiwase.App.Infrastructure.Interface;
using Heiwase.App.Shared.Enums;
using Heiwase.App.Shared.Models;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using NSubstitute;

namespace Heiwase.App.Tests.Unit.Api;

public sealed class ApplicationFormFunctionTests
{
    [Fact]
    public async Task Valid_Application_Is_Mapped_And_Saved()
    {
        var repository = Substitute.For<IApplicationFormRepository>();
        var validator = CreateValidator(true);
        repository.AddAsync(Arg.Any<ApplicationFormEntity>(), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);
        var function = new ApplicationFormFunction(repository, validator, Substitute.For<IApplicationNotifier>());
        var request = CreateRequest(new ApplicationFormDto
        {
            Name = "Teszt Elek",
            Email = "elek@example.com",
            CaptchaToken = "token",
            Sex = GenderType.Male,
            DateOfBirth = new DateOnly(1990, 5, 17),
            TrainingTypes = [TrainingType.Adult, TrainingType.Athletics]
        });

        var result = await function.SubmitAsync(request);

        Assert.IsType<OkResult>(result);
        await repository.Received(1).AddAsync(
            Arg.Is<ApplicationFormEntity>(entity =>
                entity.Name == "Teszt Elek"
                && entity.Email == "elek@example.com"
                && entity.Sex == nameof(GenderType.Male)
                && entity.DateOfBirth == new DateOnly(1990, 5, 17)
                && entity.TrainingTypes.SequenceEqual(new[] { nameof(TrainingType.Adult), nameof(TrainingType.Athletics) })
                && !entity.IsReviewed
                && entity.SubmittedAt != default),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Minor_Without_Guardian_Returns_Bad_Request_Without_Saving()
    {
        var repository = Substitute.For<IApplicationFormRepository>();
        var validator = CreateValidator(true);
        var function = new ApplicationFormFunction(repository, validator, Substitute.For<IApplicationNotifier>());
        var request = CreateRequest(new ApplicationFormDto
        {
            Name = "Teszt Elek",
            Email = "elek@example.com",
            CaptchaToken = "token",
            Sex = GenderType.Male,
            DateOfBirth = DateOnly.FromDateTime(DateTime.UtcNow).AddYears(-10)
        });

        var result = await function.SubmitAsync(request);

        Assert.IsType<BadRequestObjectResult>(result);
        await repository.DidNotReceive().AddAsync(
            Arg.Any<ApplicationFormEntity>(),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Failed_Captcha_Returns_Bad_Request_Without_Saving()
    {
        var repository = Substitute.For<IApplicationFormRepository>();
        var function = new ApplicationFormFunction(repository, CreateValidator(false), Substitute.For<IApplicationNotifier>());
        var request = CreateRequest(new ApplicationFormDto
        {
            Name = "Teszt Elek",
            Email = "elek@example.com",
            CaptchaToken = "bad",
            Sex = GenderType.Male,
            DateOfBirth = new DateOnly(1990, 5, 17)
        });

        var result = await function.SubmitAsync(request);

        Assert.IsType<BadRequestObjectResult>(result);
        await repository.DidNotReceive().AddAsync(
            Arg.Any<ApplicationFormEntity>(),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Captcha_Service_Outage_Returns_503_Without_Saving()
    {
        var repository = Substitute.For<IApplicationFormRepository>();
        var validator = Substitute.For<ITurnstileValidator>();
        validator.ValidateAsync(Arg.Any<string?>(), Arg.Any<CancellationToken>())
            .Returns<Task<bool>>(_ => throw new HttpRequestException("down"));
        var function = new ApplicationFormFunction(repository, validator, Substitute.For<IApplicationNotifier>());
        var request = CreateRequest(new ApplicationFormDto
        {
            Name = "Teszt Elek",
            Email = "elek@example.com",
            CaptchaToken = "token",
            Sex = GenderType.Male,
            DateOfBirth = new DateOnly(1990, 5, 17)
        });

        var result = await function.SubmitAsync(request);

        Assert.Equal(StatusCodes.Status503ServiceUnavailable, Assert.IsType<StatusCodeResult>(result).StatusCode);
        await repository.DidNotReceive().AddAsync(
            Arg.Any<ApplicationFormEntity>(),
            Arg.Any<CancellationToken>());
    }

    private static ITurnstileValidator CreateValidator(bool result)
    {
        var validator = Substitute.For<ITurnstileValidator>();
        validator.ValidateAsync(Arg.Any<string?>(), Arg.Any<CancellationToken>()).Returns(result);
        return validator;
    }
    private static HttpRequest CreateRequest(ApplicationFormDto application)
    {
        var context = new DefaultHttpContext();
        context.Request.ContentType = "application/json";
        context.Request.Body = new MemoryStream(JsonSerializer.SerializeToUtf8Bytes(application));
        return context.Request;
    }
}
