using System.ComponentModel.DataAnnotations;

namespace Heiwase.App.Blazor.Components.Pages.ContactSection;

public partial class ContactSection
{
    public sealed class ApplicantModel : IValidatableObject
    {
        [Required(ErrorMessage = "A jelentkező neve kötelező.")]
        public string Name { get; set; } = String.Empty;

        [Required(ErrorMessage = "Az e-mail cím megadása kötelező.")]
        [EmailAddress(ErrorMessage = "Érvénytelen e-mail cím formátum.")]
        public string Email { get; set; } = String.Empty;

        public string Phone { get; set; } = String.Empty;

        [Required(ErrorMessage = "A nem megadása kötelező.")]
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
                    "18 éven aluli jelentkező esetén a szülő/gondviselő neve kötelező.",
                    [nameof(GuardianName)]);
            }

            if ( TrainingTypes.Contains("Női önvédelem") && Sex != "nő" )
            {
                yield return new ValidationResult(
                    "A Női önvédelem edzéstípust csak női jelentkező választhatja.",
                    [nameof(TrainingTypes)]);
            }

            if ( TrainingTypes.Contains("Gyerek") && TrainingTypes.Contains("Felnőtt") )
            {
                yield return new ValidationResult(
                    "A \"Gyerek\" és \"Felnőtt\" edzéstípusok egyszerre nem választhatók.",
                    [nameof(TrainingTypes)]);
            }

            if ( DateOfBirth.HasValue && !IsMinor && TrainingTypes.Contains("Gyerek") )
            {
                yield return new ValidationResult(
                    "A \"Gyerek\" edzéstípus 18 éven felüli jelentkező számára nem választható.",
                    [nameof(TrainingTypes)]);
            }
        }
    }
}
