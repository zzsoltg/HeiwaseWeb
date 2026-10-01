using Heiwase.App.Shared.Enums;

namespace Heiwase.App.Shared.Models;

public class ApplicationFormDto
{
    public string Name { get; set; } = String.Empty;
    public string Email { get; set; } = String.Empty;
    public string Phone { get; set; } = String.Empty;
    public GenderType Sex { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public string GuardianName { get; set; } = String.Empty;
    public List<TrainingType> TrainingTypes { get; set; } = [ ];
    public string Message { get; set; } = String.Empty;
}
