using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

using Radzen;

using System.Net.Http.Json;
using System.Timers;
using Microsoft.Extensions.Localization;
using Heiwase.App.Blazor.Components.Shared.Dialogs.CompetitorResultsDialog;
using Heiwase.App.Blazor.Components.Shared.Dialogs.SenpaiResultsDialog;
using System.Globalization;
using Heiwase.App.Blazor.Helpers;
using Heiwase.App.Shared.Models;

namespace Heiwase.App.Blazor.Components.Sections.Home.HallOfFameSection;

public partial class HallOfFameSection : IAsyncDisposable
{
    [Inject]
    public IStringLocalizer<HallOfFameSectionResource> L { get; set; } = default!;
    [Inject]
    public IJSRuntime JS { get; set; } = default!;

    [Inject]
    public HttpClient Http { get; set; } = default!;

    [Inject]
    public DialogService DialogService { get; set; } = default!;

    protected IJSObjectReference? Module { get; set; }
    protected List<HallOfFameMemberDto> Competitors { get; set; } = [];
    protected List<HallOfFameMemberDto> Senpais { get; set; } = [];
    protected System.Timers.Timer? Timer { get; set; }
    protected System.Timers.Timer? ResumeTimer { get; set; }

    protected bool TrackInitialized { get; set; } = false;
    protected bool TrackResetPending { get; set; } = false;
    protected bool IsAnimating { get; set; } = false;
    protected bool UserInteractionPaused { get; set; } = false;
    protected int CompetitorIndex { get; set; } = 0;
    protected int SenpaiIndex { get; set; } = 0;

    protected static string HallOfFameDataString
        => CultureInfo.CurrentCulture.Name ==  "hu-HU" ? "data/halloffame.json" : "data/halloffameen.json";
    protected const string CompetitorGridId = "competitors-grid";
    protected const string SenpaiGridId = "senpais-grid";

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if ( firstRender )
        {
            Module = await JS.InvokeAsync<IJSObjectReference>("import", "./js/animations.js");

            if ( Module is not null )
            {
                await Module.InvokeVoidAsync("initAnimations");
            }
        }

        if ( !TrackInitialized && Module is not null && Competitors.Count > 0 )
        {
            TrackInitialized = true;
            await Module.InvokeVoidAsync("initTrack", CompetitorGridId);
            await Module.InvokeVoidAsync("initTrack", SenpaiGridId);
            StartTimer();
        }

        if ( TrackResetPending && Module is not null )
        {
            TrackResetPending = false;
            await Module.InvokeVoidAsync("resetTrack", CompetitorGridId);
            await Module.InvokeVoidAsync("resetTrack", SenpaiGridId);
            IsAnimating = false;

            if ( !UserInteractionPaused )
            {
                Timer?.Start();
            }
        }
    }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            var data = await Http.GetFromJsonAsync<HallOfFameDataDto>(HallOfFameDataString);

            if ( data != null )
            {
                Competitors = data.Competitors;
                Senpais = data.Senpais;
            }
        }
        catch ( Exception ex )
        {
            Console.WriteLine($"Hiba a JSON betöltésekor: {ex.Message}");
        }
    }

    protected void StartTimer()
    {
        Timer = new System.Timers.Timer(3000);
        Timer.Elapsed += OnTimerElapsed;
        Timer.AutoReset = true;
        Timer.Enabled = true;
    }

    protected async void OnTimerElapsed(object? sender, ElapsedEventArgs e)
    {
        Timer!.Stop();
        IsAnimating = true;

        if ( Module is not null )
        {
            var compTask = Module.InvokeAsync<object>("slideTrackRight", CompetitorGridId).AsTask();
            var senpTask = Module.InvokeAsync<object>("slideTrackLeft", SenpaiGridId).AsTask();
            await Task.WhenAll(compTask, senpTask);
        }

        PrevCompetitor();
        NextSenpai();
        TrackResetPending = true;
        await InvokeAsync(StateHasChanged);
    }

    public async ValueTask DisposeAsync()
    {
        Timer?.Dispose();
        ResumeTimer?.Dispose();
        GC.SuppressFinalize(this);

        if ( Module is not null )
        {
            await Module.DisposeAsync();
        }
    }

    protected List<HallOfFameMemberDto> GetCompetitorItems()
    {
        if ( Competitors.Count == 0 )
        {
            return [];
        }

        int count = Competitors.Count;
        return
        [
            Competitors[( CompetitorIndex - 1 + count ) % count],
            Competitors[CompetitorIndex % count],
            Competitors[( CompetitorIndex + 1 ) % count],
            Competitors[( CompetitorIndex + 2 ) % count],
            Competitors[( CompetitorIndex + 3 ) % count]
        ];
    }

    protected List<HallOfFameMemberDto> GetSenpaiItems()
    {
        if ( Senpais.Count == 0 )
        {
            return [];
        }

        int count = Senpais.Count;
        return
        [
            Senpais[( SenpaiIndex - 1 + count ) % count],
            Senpais[SenpaiIndex % count],
            Senpais[( SenpaiIndex + 1 ) % count],
            Senpais[( SenpaiIndex + 2 ) % count],
            Senpais[( SenpaiIndex + 3 ) % count]
        ];
    }

    protected void PauseAutoAnimation()
    {
        UserInteractionPaused = true;
        Timer?.Stop();

        ResumeTimer?.Stop();
        ResumeTimer?.Dispose();
        ResumeTimer = new System.Timers.Timer(15_000);
        ResumeTimer.Elapsed  += OnResumeTimerElapsed;
        ResumeTimer.AutoReset = false;
        ResumeTimer.Enabled   = true;
    }

    protected void OnResumeTimerElapsed(object? sender, ElapsedEventArgs e)
    {
        UserInteractionPaused = false;

        if ( !IsAnimating )
        {
            Timer?.Start();
        }
    }

    protected Task OnCompetitorLeftClick() 
        => PerformCompetitorSlide(slidesLeft: true);

    protected Task OnCompetitorRightClick()
        => PerformCompetitorSlide(slidesLeft: false);

    protected Task OnSenpaiLeftClick()
        => PerformSenpaiSlide(slidesLeft: true);

    protected Task OnSenpaiRightClick()
        => PerformSenpaiSlide(slidesLeft: false);

    protected async Task PerformCompetitorSlide(bool slidesLeft)
    {
        PauseAutoAnimation();

        if ( IsAnimating )
        {
            return;
        }

        IsAnimating = true;

        if ( Module is not null )
        {
            await Module.InvokeAsync<object>(slidesLeft ? "slideTrackLeft" : "slideTrackRight", CompetitorGridId);
        }

        if ( slidesLeft )
        {
            NextCompetitor();
        }
        else
        {
            PrevCompetitor();
        }

        TrackResetPending = true;
        await InvokeAsync(StateHasChanged);
    }

    protected async Task PerformSenpaiSlide(bool slidesLeft)
    {
        PauseAutoAnimation();

        if ( IsAnimating )
        {
            return;
        }

        IsAnimating = true;

        if ( Module is not null )
        {
            await Module.InvokeAsync<object>(slidesLeft ? "slideTrackLeft" : "slideTrackRight", SenpaiGridId);
        }

        if ( slidesLeft )
        {
            NextSenpai();
        }
        else
        {
            PrevSenpai();
        }

        TrackResetPending = true;
        await InvokeAsync(StateHasChanged);
    }

    protected void NextCompetitor()
    {
        if ( Competitors.Count > 0 )
        {
            CompetitorIndex = ( CompetitorIndex + 1 ) % Competitors.Count;
        }
    }

    protected void PrevCompetitor()
    {
        if ( Competitors.Count > 0 )
        {
            CompetitorIndex = ( CompetitorIndex - 1 + Competitors.Count ) % Competitors.Count;
        }
    }

    protected void NextSenpai()
    {
        if ( Senpais.Count > 0 )
        {
            SenpaiIndex = ( SenpaiIndex + 1 ) % Senpais.Count;
        }
    }

    protected void PrevSenpai()
    {
        if ( Senpais.Count > 0 )
        {
            SenpaiIndex = ( SenpaiIndex - 1 + Senpais.Count ) % Senpais.Count;
        }
    }

    protected Task OpenCompetitorResultsDialogAsync(HallOfFameMemberDto member) =>
        DialogService.OpenAsync<CompetitorResultsDialog>(
            $"{member.Name}{L["Achievements"]}",
            new Dictionary<string, object?> { { "Member", member } },
            DialogDefaults.Options());

    protected Task OpenSenpaiResultsDialogAsync(HallOfFameMemberDto member) =>
        DialogService.OpenAsync<SenpaiResultsDialog>(
            $"{member.Name}{L["Achievements"]}",
            new Dictionary<string, object?> { { "Member", member } },
            DialogDefaults.Options());
}
