using System.Globalization;
using System.Net.Http.Json;

using Heiwase.App.Blazor.Configuration;
using Heiwase.App.Blazor.Services.Interface;
using Heiwase.App.Blazor.ViewModels;
using Heiwase.App.Shared.Enums;
using Heiwase.App.Shared.Models;

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

namespace Heiwase.App.Blazor.Components.Sections.Home.ContactSection;

public partial class ContactSection : IAsyncDisposable
{
    protected const string CaptchaElementId = "contact-turnstile";

    private IJSObjectReference? _module;
    private DotNetObjectReference<ContactSection>? _selfRef;
    private string? _widgetId;
    private string _captchaToken = string.Empty;

    [Inject]
    public IJSRuntime JS { get; set; } = default!;

    [Inject]
    public TurnstileSettings Turnstile { get; set; } = default!;

    protected bool CaptchaError { get; set; }

    [Inject]
    public IApplicationFormService ApplicationFormService { get; set; } = default!;

    [Inject]
    public IStringLocalizer<ContactSectionResource> L { get; set; } = default!;

    [Inject]
    public ILogger<ContactSection> Logger { get; set; } = default!;

    protected ApplicantModel Applicant { get; set; } = new();

    protected bool FormSubmitted { get; set; }

    protected bool FormError { get; set; }

    protected TrainingType[] TrainingTypeOptions { get; set; } = [];

    protected override void OnInitialized()
    {
        TrainingTypeOptions = [
            TrainingType.Child,
            TrainingType.Adult,
            TrainingType.Sportkarate,
            TrainingType.WomenSelfDefense,
            TrainingType.Athletics
        ];
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender || GetType() == typeof(ContactSection))
        {
            // The wrapper component renders a language variant that inherits this class and owns the form.
            return;
        }

        try
        {
            _module = await JS.InvokeAsync<IJSObjectReference>("import", "./js/turnstile.js");
            _selfRef = DotNetObjectReference.Create(this);
            _widgetId = await _module.InvokeAsync<string>(
                "render",
                CaptchaElementId,
                Turnstile.SiteKey,
                CultureInfo.CurrentUICulture.TwoLetterISOLanguageName,
                _selfRef);
        }
        catch (JSException)
        {
            // Script blocked or failed to load; submitting will show the security check error.
        }
    }

    [JSInvokable]
    public void OnCaptchaSolved(string token)
    {
        _captchaToken = token;
        CaptchaError = false;
        _ = InvokeAsync(StateHasChanged);
    }

    [JSInvokable]
    public void OnCaptchaExpired() => _captchaToken = string.Empty;

    public async ValueTask DisposeAsync()
    {
        if (_module is not null)
        {
            try
            {
                if (_widgetId is not null)
                {
                    await _module.InvokeVoidAsync("remove", _widgetId);
                }

                await _module.DisposeAsync();
            }
            catch (JSDisconnectedException)
            {
            }
        }

        _selfRef?.Dispose();
        GC.SuppressFinalize(this);
    }

    protected string GetLocalizedTrainingName(TrainingType type) =>
        type switch
        {
            TrainingType.Child => L["Child"],
            TrainingType.Adult => L["Adult"],
            TrainingType.Sportkarate => L["Sportkarate"],
            TrainingType.WomenSelfDefense => L["SelfDefense"],
            TrainingType.Athletics => L["Athletics"],
            _ => string.Empty
        };

    protected async Task HandleValidSubmit()
    {
        FormSubmitted = false;
        FormError = false;
        CaptchaError = false;

        if (string.IsNullOrEmpty(_captchaToken))
        {
            CaptchaError = true;
            return;
        }

        var application = new ApplicationFormDto
        {
            Name = Applicant.Name,
            Email = Applicant.Email,
            Phone = Applicant.Phone,
            Sex = Applicant.Sex,
            DateOfBirth = Applicant.DateOfBirth,
            GuardianName = Applicant.GuardianName,
            TrainingTypes = [.. Applicant.TrainingTypes],
            Message = Applicant.Message,
            CaptchaToken = _captchaToken
        };

        try
        {
            var success = await ApplicationFormService.SubmitAsync(application);

            if (success)
            {
                FormSubmitted = true;
                Applicant = new ApplicantModel();
            }
            else
            {
                FormError = true;
            }
        }
        catch (HttpRequestException)
        {
            FormError = true;
        }
        finally
        {
            await ResetCaptchaAsync();
        }
    }

    private async Task ResetCaptchaAsync()
    {
        _captchaToken = string.Empty;

        if (_module is not null && _widgetId is not null)
        {
            await _module.InvokeVoidAsync("reset", _widgetId);
        }
    }

    protected bool IsTrainingTypeDisabled(TrainingType type)
    {
        if (type == TrainingType.WomenSelfDefense)
        {
            return Applicant.Sex == GenderType.Male;
        }

        if (type == TrainingType.Child)
        {
            return (Applicant.DateOfBirth.HasValue && !Applicant.IsMinor)
                   || Applicant.TrainingTypes.Contains(TrainingType.Adult);
        }

        if (type == TrainingType.Adult)
        {
            return Applicant.TrainingTypes.Contains(TrainingType.Child);
        }

        return false;
    }

    protected void OnTrainingTypeChanged(TrainingType type, bool isChecked)
    {
        if (isChecked)
        {
            if (!IsTrainingTypeDisabled(type) && !Applicant.TrainingTypes.Contains(type))
            {
                Applicant.TrainingTypes.Add(type);
            }
        }
        else
        {
            Applicant.TrainingTypes.Remove(type);
        }
    }

    protected void SanitizeTrainingTypes() =>
        Applicant.TrainingTypes.RemoveAll(IsTrainingTypeDisabled);
}
