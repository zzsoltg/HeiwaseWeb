using Heiwase.App.Infrastructure.Entities;

using Microsoft.Extensions.Logging;

namespace Heiwase.App.Api.Services;

/// <summary>Used when no Azure Communication Services endpoint is configured (e.g. local development).</summary>
public sealed partial class NoOpApplicationNotifier : IApplicationNotifier
{
    private readonly ILogger<NoOpApplicationNotifier> _logger;

    public NoOpApplicationNotifier(ILogger<NoOpApplicationNotifier> logger)
    {
        ArgumentNullException.ThrowIfNull(logger);
        _logger = logger;
    }

    public Task NotifyAsync(ApplicationFormEntity application, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(application);
        LogSkipped(_logger, application.Id);
        return Task.CompletedTask;
    }

    [LoggerMessage(Level = LogLevel.Information, Message = "Email:Endpoint is not configured; no notification sent for application {ApplicationId}.")]
    private static partial void LogSkipped(ILogger logger, string applicationId);
}
