using Azure;
using Azure.Communication.Email;
using Azure.Identity;

using Heiwase.App.Api.Configuration;
using Heiwase.App.Infrastructure.Entities;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Heiwase.App.Api.Services;

public sealed partial class AcsApplicationNotifier : IApplicationNotifier
{
    private readonly EmailClient _client;
    private readonly EmailOptions _options;
    private readonly ILogger<AcsApplicationNotifier> _logger;

    public AcsApplicationNotifier(EmailClient client, IOptions<EmailOptions> options, ILogger<AcsApplicationNotifier> logger)
    {
        ArgumentNullException.ThrowIfNull(client);
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(logger);

        _client = client;
        _options = options.Value;
        _logger = logger;
    }

    public async Task NotifyAsync(ApplicationFormEntity application, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(application);

        var recipients = _options.GetRecipients();
        if (recipients.Count == 0 || string.IsNullOrWhiteSpace(_options.SenderAddress))
        {
            LogNotConfigured(_logger);
            return;
        }

        EmailContent content = new(ApplicationEmailBuilder.BuildSubject(application))
        {
            PlainText = ApplicationEmailBuilder.BuildPlainText(application),
            Html = ApplicationEmailBuilder.BuildHtml(application)
        };

        EmailMessage message = new(
            _options.SenderAddress,
            new EmailRecipients([.. recipients.Select(address => new EmailAddress(address))]),
            content);

        if (!string.IsNullOrWhiteSpace(application.Email))
        {
            message.ReplyTo.Add(new EmailAddress(application.Email, application.Name));
        }

        try
        {
            await _client.SendAsync(WaitUntil.Started, message, cancellationToken).ConfigureAwait(false);
        }
        catch (Exception exception) when (exception is RequestFailedException or AuthenticationFailedException or OperationCanceledException)
        {
            LogSendFailed(_logger, exception, application.Id);
        }
    }

    [LoggerMessage(Level = LogLevel.Warning, Message = "Application email is not configured (sender or recipients missing); skipping notification.")]
    private static partial void LogNotConfigured(ILogger logger);

    [LoggerMessage(Level = LogLevel.Error, Message = "Failed to send the notification email for application {ApplicationId}.")]
    private static partial void LogSendFailed(ILogger logger, Exception exception, string applicationId);
}
