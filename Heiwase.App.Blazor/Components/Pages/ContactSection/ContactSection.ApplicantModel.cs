using System.ComponentModel.DataAnnotations;

namespace Heiwase.App.Blazor.Components.Pages.ContactSection;

public partial class ContactSection
{
    public sealed class ApplicantModel : IValidatableObject
    {
        [Required(
            ErrorMessageResourceType = typeof(ContactSectionResource),
            ErrorMessageResourceName = nameof(ContactSectionResource.MandatoryName))]
        public string Name { get; set; } = String.Empty;

        [Required(
            ErrorMessageResourceType = typeof(ContactSectionResource),
            ErrorMessageResourceName = nameof(ContactSectionResource.MandatoryEmail))]
        [EmailAddress(
            ErrorMessageResourceType = typeof(ContactSectionResource),
            ErrorMessageResourceName = nameof(ContactSectionResource.InvalidEmail))]
        public string Email { get; set; } = String.Empty;

        public string Phone { get; set; } = String.Empty;

        [Required(
            ErrorMessageResourceType = typeof(ContactSectionResource),
            ErrorMessageResourceName = nameof(ContactSectionResource.MandatorySex))]
        public string Sex { get; set; } = String.Empty;

        public DateOnly? DateOfBirth { get; set; }

        public string GuardianName { get; set; } = String.Empty;

        public List<string> TrainingTypes { get; set; } = [];

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
                var age   = today.Year - DateOfBirth.Value.Year;

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
                    ContactSectionResource.Under18Guradian,
                    [nameof(GuardianName)]);
            }

            if ( TrainingTypes.Contains("Női önvédelem") && Sex != "nő" )
            {
                yield return new ValidationResult(
                    ContactSectionResource.MenSelfDefenseApplication,
                    [nameof(TrainingTypes)]);
            }

            if ( TrainingTypes.Contains("Gyerek") && TrainingTypes.Contains("Felnőtt") )
            {
                yield return new ValidationResult(
                    ContactSectionResource.NoSameTime,
                    [nameof(TrainingTypes)]);
            }

            if ( DateOfBirth.HasValue && !IsMinor && TrainingTypes.Contains("Gyerek") )
            {
                yield return new ValidationResult(
                    ContactSectionResource.Above18,
                    [nameof(TrainingTypes)]);
            }
        }
    }
}
