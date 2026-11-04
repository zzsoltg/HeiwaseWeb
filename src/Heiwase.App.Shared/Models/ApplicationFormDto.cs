using System.ComponentModel.DataAnnotations;

using Heiwase.App.Shared.Enums;

namespace Heiwase.App.Shared.Models;

public sealed class ApplicationFormDto : IValidatableObject
{
    [Required]
    public string Name { get; set; } = string.Empty;

    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public GenderType Sex { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public string GuardianName { get; set; } = string.Empty;

    public List<TrainingType> TrainingTypes { get; set; } = [];

    public string Message { get; set; } = string.Empty;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (!Enum.IsDefined(Sex) || Sex == GenderType.Unknown)
        {
            yield return new ValidationResult("Select a valid sex.", [nameof(Sex)]);
        }

        if (DateOfBirth is { } dateOfBirth)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            if (dateOfBirth > today)
            {
                yield return new ValidationResult("Date of birth cannot be in the future.", [nameof(DateOfBirth)]);
            }
            else if (IsMinor(dateOfBirth, today) && string.IsNullOrWhiteSpace(GuardianName))
            {
                yield return new ValidationResult("A guardian name is required for applicants under 18.", [nameof(GuardianName)]);
            }
        }

        if (TrainingTypes is null)
        {
            yield return new ValidationResult("Select valid training types.", [nameof(TrainingTypes)]);
        }
        else
        {
            foreach (var trainingType in TrainingTypes)
            {
                if (!Enum.IsDefined(trainingType) || trainingType == TrainingType.Unknown)
                {
                    yield return new ValidationResult("Select valid training types.", [nameof(TrainingTypes)]);
                    break;
                }
            }
        }
    }

    private static bool IsMinor(DateOnly dateOfBirth, DateOnly today)
    {
        var age = today.Year - dateOfBirth.Year;
        if (today < dateOfBirth.AddYears(age))
        {
            age--;
        }

        return age < 18;
    }
}
