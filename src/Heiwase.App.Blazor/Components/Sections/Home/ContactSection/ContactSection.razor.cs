using Heiwase.App.Blazor.ViewModels;
using Heiwase.App.Shared.Enums;

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

using System.Net.Http.Json;

namespace Heiwase.App.Blazor.Components.Sections.Home.ContactSection;

public partial class ContactSection
{
    [Inject]
    public HttpClient Http { get; set; } = default!;
    [Inject]
    public IStringLocalizer<ContactSectionResource> L { get; set; } = default!;
    protected ApplicantModel Applicant { get; set; } = new( );

    protected bool FormSubmitted { get; set; } = false;
    protected bool FormError { get; set; } = false;

    protected const string FormspreeEndpoint = "https://formspree.io/f/mrenpyzo";

    protected TrainingType[ ] TrainingTypeOptions { get; set; } = [ ];

    protected override void OnInitialized( )
    {
        TrainingTypeOptions = [
            TrainingType.Child,
            TrainingType.Adult,
            TrainingType.Sportkarate,
            TrainingType.WomenSelfDefense,
            TrainingType.Athletics
        ];
    }

    protected string GetLocalizedTrainingName(TrainingType type) =>
        type switch
        {
            TrainingType.Child => L[ "Child" ],
            TrainingType.Adult => L[ "Adult" ],
            TrainingType.Sportkarate => L[ "Sportkarate" ],
            TrainingType.WomenSelfDefense => L[ "SelfDefense" ],
            TrainingType.Athletics => L[ "Athletics" ],
            _ => string.Empty
        };

    protected async Task HandleValidSubmit( )
    {
        FormSubmitted = false;
        FormError = false;

        var payload = new
        {
            name = Applicant.Name,
            email = Applicant.Email,
            phone = Applicant.Phone,
            sex = Applicant.Sex,
            dateOfBirth = Applicant.DateOfBirth?.ToString("yyyy-MM-dd"),
            guardianName = Applicant.GuardianName,
            trainingTypes = string.Join(", ", Applicant.TrainingTypes),
            message = Applicant.Message
        };

        var response = await Http.PostAsJsonAsync(FormspreeEndpoint, payload);

        if ( response.IsSuccessStatusCode )
        {
            FormSubmitted = true;
            Applicant = new ApplicantModel( );
        }
        else
        {
            FormError = true;
        }
    }

    protected bool IsTrainingTypeDisabled(TrainingType type)
    {
        if ( type == TrainingType.WomenSelfDefense )
        {
            return Applicant.Sex == GenderType.Male;
        }

        if ( type == TrainingType.Child )
        {
            return ( Applicant.DateOfBirth.HasValue && !Applicant.IsMinor )
                   || Applicant.TrainingTypes.Contains(TrainingType.Adult);
        }

        if ( type == TrainingType.Adult )
        {
            return Applicant.TrainingTypes.Contains(TrainingType.Child);
        }

        return false;
    }

    protected void OnTrainingTypeChanged(TrainingType type, bool isChecked)
    {
        if ( isChecked )
        {
            if ( !IsTrainingTypeDisabled(type) && !Applicant.TrainingTypes.Contains(type) )
                Applicant.TrainingTypes.Add(type);
        }
        else
        {
            Applicant.TrainingTypes.Remove(type);
        }
    }

    protected void SanitizeTrainingTypes( ) =>
        Applicant.TrainingTypes.RemoveAll(t => IsTrainingTypeDisabled(t));
}
