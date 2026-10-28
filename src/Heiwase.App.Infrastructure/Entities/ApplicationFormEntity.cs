using System.Text.Json.Serialization;

namespace Heiwase.App.Infrastructure.Entities;

public sealed class ApplicationFormEntity : BaseEntity
{
    [JsonPropertyName("applicantName")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;

    [JsonPropertyName("phone")]
    public string Phone { get; set; } = string.Empty;

    [JsonPropertyName("sex")]
    public string Sex { get; set; } = string.Empty;

    [JsonPropertyName("dateOfBirth")]
    public DateOnly? DateOfBirth { get; set; }

    [JsonPropertyName("guardianName")]
    public string GuardianName { get; set; } = string.Empty;

    [JsonPropertyName("trainingTypes")]
    public List<string> TrainingTypes { get; set; } = [];

    [JsonPropertyName("message")]
    public string Message { get; set; } = string.Empty;

    [JsonPropertyName("submittedAt")]
    public DateTime SubmittedAt { get; set; }

    [JsonPropertyName("isReviewed")]
    public bool IsReviewed { get; set; }
}
