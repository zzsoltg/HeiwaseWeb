using System.Text.Json.Serialization;

namespace Heiwase.App.Infrasturcture.Entities;

public class ApplicationForm
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("applicantName")]
    public string Name { get; set; } = String.Empty;

    [JsonPropertyName("email")]
    public string Email { get; set; } = String.Empty;

    [JsonPropertyName("phone")]
    public string Phone { get; set; } = String.Empty;

    [JsonPropertyName("sex")]
    public string Sex { get; set; } = String.Empty;

    [JsonPropertyName("dateOfBirth")]
    public DateOnly? DateOfBirth { get; set; }

    [JsonPropertyName("guardianName")]
    public string GuardianName { get; set; } = String.Empty;

    [JsonPropertyName("trainingTypes")]

    public List<string> TrainingTypes { get; set; } = [ ];

    [JsonPropertyName("message")]
    public string Message { get; set; } = String.Empty;
}
