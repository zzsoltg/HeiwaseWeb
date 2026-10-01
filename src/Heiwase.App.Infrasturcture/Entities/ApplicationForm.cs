namespace Heiwase.App.Infrasturcture.Entities;

public class ApplicationForm
{
    public string Name { get; set; } = String.Empty;
    public string Email { get; set; } = String.Empty;
    public string Phone { get; set; } = String.Empty;
    public string Sex { get; set; } = String.Empty;
    public DateOnly? DateOfBirth { get; set; }
    public string GuardianName { get; set; } = String.Empty;
    public List<string> TrainingTypes { get; set; } = [ ];
    public string Message { get; set; } = String.Empty;
}
