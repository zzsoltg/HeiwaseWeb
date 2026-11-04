namespace Heiwase.App.Api.Configuration;

public sealed class EmailOptions
{
    public const string SectionName = "Email";

    /// <summary>Azure Communication Services endpoint, e.g. https://acs-heiwase.europe.communication.azure.com. Empty disables sending.</summary>
    public string Endpoint { get; set; } = string.Empty;

    /// <summary>Verified sender address of the linked email domain, e.g. DoNotReply@xxxx.azurecomm.net.</summary>
    public string SenderAddress { get; set; } = string.Empty;

    /// <summary>Admin recipients separated by ';' or ','.</summary>
    public string Recipients { get; set; } = string.Empty;

    public IReadOnlyList<string> GetRecipients()
        => [.. Recipients.Split([';', ','], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)];
}
