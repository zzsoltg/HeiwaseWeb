using Heiwase.App.Shared.Enums;

using System.ComponentModel.DataAnnotations;

namespace Heiwase.App.Blazor.ViewModels;

public sealed class ApplicantModel : IValidatableObject
{
    [Required(
        ErrorMessageResourceType = typeof(ApplicantModelResource),
        ErrorMessageResourceName = nameof(ApplicantModelResource.MandatoryName))]
    public string Name { get; set; } = String.Empty;

    [Required(
        ErrorMessageResourceType = typeof(ApplicantModelResource),
        ErrorMessageResourceName = nameof(ApplicantModelResource.MandatoryEmail))]
    [EmailAddress(
        ErrorMessageResourceType = typeof(ApplicantModelResource),
        ErrorMessageResourceName = nameof(ApplicantModelResource.InvalidEmail))]
    public string Email { get; set; } = String.Empty;

    public string Phone { get; set; } = String.Empty;

    [Required(
        ErrorMessageResourceType = typeof(ApplicantModelResource),
        ErrorMessageResourceName = nameof(ApplicantModelResource.MandatorySex))]
    public GenderType Sex { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public string GuardianName { get; set; } = String.Empty;

    public List<TrainingType> TrainingTypes { get; set; } = [ ];

    public string Message { get; set; } = String.Empty;

    public bool IsMinor
    {
        get
        {
            if ( !DateOfBirth.HasValue )
            {
                return false;
            }

            var today = DateOnly.FromDateTime(DateTime.Today);
            var age = today.Year - DateOfBirth.Value.Year;

            if ( today < DateOfBirth.Value.AddYears(age) )
            {
                age--;
            }

            return age < 18;
        }
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if ( IsMinor && string.IsNullOrWhiteSpace(GuardianName) )
        {
            yield return new ValidationResult(
                ApplicantModelResource.Under18Guradian,
                [ nameof(GuardianName) ]);
        }

        if ( TrainingTypes.Contains(TrainingType.WomenSelfDefense) && Sex != GenderType.Female )
        {
            yield return new ValidationResult(
                ApplicantModelResource.MenSelfDefenseApplication,
                [ nameof(TrainingTypes) ]);
        }

        if ( TrainingTypes.Contains(TrainingType.Child) && TrainingTypes.Contains(TrainingType.Adult) )
        {
            yield return new ValidationResult(
                ApplicantModelResource.NoSameTime,
                [ nameof(TrainingTypes) ]);
        }

        if ( DateOfBirth.HasValue && !IsMinor && TrainingTypes.Contains(TrainingType.Child) )
        {
            yield return new ValidationResult(
                ApplicantModelResource.Above18,
                [ nameof(TrainingTypes) ]);
        }
    }
}