using Microsoft.AspNetCore.Components;

using System.Net.Http.Json;

namespace Heiwase.App.Blazor.Components.Pages.ContactSection;

public partial class ContactSection
{
    [Inject]
    public HttpClient Http { get; set; } = default!;

    private ApplicantModel _applicant = new();

    private bool _formSubmitted = false;
    private bool _formError = false;

    private const string FormspreeEndpoint = "https://formspree.io/f/mrenpyzo";
    private const string Woman = "nő";
    private const string Adult = "Felnőtt";
    private const string Child = "Gyerek";

    private static readonly string[] TrainingTypeOptions =
        ["Felnőtt", "Gyerek", "Sportkarate", "Női önvédelem", "Atlétika"];

    private async Task HandleValidSubmit()
    {
        _formSubmitted = false;
        _formError = false;

        var payload = new
        {
            name = _applicant.Name,
            email = _applicant.Email,
            phone = _applicant.Phone,
            sex = _applicant.Sex,
            dateOfBirth = _applicant.DateOfBirth?.ToString("yyyy-MM-dd"),
            guardianName = _applicant.GuardianName,
            trainingTypes = string.Join(", ", _applicant.TrainingTypes),
            message = _applicant.Message
        };

        var response = await Http.PostAsJsonAsync(FormspreeEndpoint, payload);

        if ( response.IsSuccessStatusCode )
        {
            _formSubmitted = true;
            _applicant = new ApplicantModel();
        }
        else
        {
            _formError = true;
        }
    }

    private bool IsTrainingTypeDisabled(string type) => type switch
    {
        "Női önvédelem" => _applicant.Sex != Woman && _applicant.Sex != String.Empty,
        "Gyerek" => ( _applicant.DateOfBirth.HasValue && !_applicant.IsMinor )
                    || _applicant.TrainingTypes.Contains(Adult),
        "Felnőtt" => _applicant.TrainingTypes.Contains(Child),
        _ => false
    };

    private void OnTrainingTypeChanged(string type, bool isChecked)
    {
        if ( isChecked )
        {
            if ( !IsTrainingTypeDisabled(type) && !_applicant.TrainingTypes.Contains(type) )
                _applicant.TrainingTypes.Add(type);
        }
        else
        {
            _applicant.TrainingTypes.Remove(type);
        }
    }

    private void SanitizeTrainingTypes() =>
        _applicant.TrainingTypes.RemoveAll(t => IsTrainingTypeDisabled(t));
}
