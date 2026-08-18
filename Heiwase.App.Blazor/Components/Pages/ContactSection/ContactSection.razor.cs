using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

using System.Net.Http.Json;

namespace Heiwase.App.Blazor.Components.Pages.ContactSection;

public partial class ContactSection
{
    [Inject]
    public HttpClient Http { get; set; } = default!;
    [Inject]
    public IStringLocalizer<ContactSectionResource> L { get; set; } = default!;

    protected ApplicantModel _applicant = new();

    protected bool _formSubmitted = false;
    protected bool _formError = false;

    protected const string FormspreeEndpoint = "https://formspree.io/f/mrenpyzo";

    protected string[] TrainingTypeOptions = [];

    protected override void OnInitialized()
    {
        TrainingTypeOptions = [
            L["Adult"],
            L["Child"],
            L["Sportkarate"],
            L["SelfDefense"],
            L["Athletics"]
        ];
    }

    protected async Task HandleValidSubmit()
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

    protected bool IsTrainingTypeDisabled(string type)
    {
        if ( type == L["Woman"].Value )
        {
            return _applicant.Sex != L["Woman"].Value && !string.IsNullOrEmpty(_applicant.Sex);
        }

        if ( type == L["Child"].Value )
        {
            return ( _applicant.DateOfBirth.HasValue && !_applicant.IsMinor )
                   || _applicant.TrainingTypes.Contains(L["Adult"].Value);
        }

        if ( type == L["Adult"].Value )
        {
            return _applicant.TrainingTypes.Contains(L["Child"].Value);
        }

        return false;
    }

    protected void OnTrainingTypeChanged(string type, bool isChecked)
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

    protected void SanitizeTrainingTypes() =>
        _applicant.TrainingTypes.RemoveAll(t => IsTrainingTypeDisabled(t));
}
