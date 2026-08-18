using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

using Heiwase.App.Blazor.Components.Shared;

using Radzen;

using System.Net.Http.Json;
using System.Timers;
using Microsoft.Extensions.Localization;

namespace Heiwase.App.Blazor.Components.Pages.HallOfFameSection;

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

    protected IJSObjectReference? _module;
    protected List<Member> _competitors = [];
    protected List<Member> _senpais = [];
    protected System.Timers.Timer? _timer;
    protected System.Timers.Timer? _resumeTimer;

    protected bool _trackInitialized = false;
    protected bool _trackResetPending = false;
    protected bool _isAnimating = false;
    protected bool _userInteractionPaused = false;
    protected int _competitorIndex = 0;
    protected int _senpaiIndex = 0;

    protected const string HallOfFameDataString = "data/halloffame.json";
    protected const string CompetitorGridId = "competitors-grid";
    protected const string SenpaiGridId = "senpais-grid";

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if ( firstRender )
        {
            _module = await JS.InvokeAsync<IJSObjectReference>("import", "./js/animations.js");

            if ( _module is not null )
            {
                await _module.InvokeVoidAsync("initAnimations");
            }
        }

        if ( !_trackInitialized && _module is not null && _competitors.Count > 0 )
        {
            _trackInitialized = true;
            await _module.InvokeVoidAsync("initTrack", CompetitorGridId);
            await _module.InvokeVoidAsync("initTrack", SenpaiGridId);
            StartTimer();
        }

        if ( _trackResetPending && _module is not null )
        {
            _trackResetPending = false;
            await _module.InvokeVoidAsync("resetTrack", CompetitorGridId);
            await _module.InvokeVoidAsync("resetTrack", SenpaiGridId);
            _isAnimating = false;

            if ( !_userInteractionPaused )
            {
                _timer?.Start();
            }
        }
    }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            var data = await Http.GetFromJsonAsync<HallOfFameData>(HallOfFameDataString);

            if ( data != null )
            {
                _competitors = data.Competitors;
                _senpais = data.Senpais;
            }
        }
        catch ( Exception ex )
        {
            Console.WriteLine($"Hiba a JSON betöltésekor: {ex.Message}");
        }
    }

    protected void StartTimer()
    {
        _timer = new System.Timers.Timer(3000);
        _timer.Elapsed += OnTimerElapsed;
        _timer.AutoReset = true;
        _timer.Enabled = true;
    }

    protected async void OnTimerElapsed(object? sender, ElapsedEventArgs e)
    {
        _timer!.Stop();
        _isAnimating = true;

        if ( _module is not null )
        {
            var compTask = _module.InvokeAsync<object>("slideTrackRight", CompetitorGridId).AsTask();
            var senpTask = _module.InvokeAsync<object>("slideTrackLeft", SenpaiGridId).AsTask();
            await Task.WhenAll(compTask, senpTask);
        }

        PrevCompetitor();
        NextSenpai();
        _trackResetPending = true;
        await InvokeAsync(StateHasChanged);
    }

    public async ValueTask DisposeAsync()
    {
        _timer?.Dispose();
        _resumeTimer?.Dispose();
        GC.SuppressFinalize(this);

        if ( _module is not null )
        {
            await _module.DisposeAsync();
        }
    }

    protected List<Member> GetCompetitorItems()
    {
        if ( _competitors.Count == 0 )
        {
            return [];
        }

        int count = _competitors.Count;
        return
        [
            _competitors[( _competitorIndex - 1 + count ) % count],
            _competitors[_competitorIndex % count],
            _competitors[( _competitorIndex + 1 ) % count],
            _competitors[( _competitorIndex + 2 ) % count],
            _competitors[( _competitorIndex + 3 ) % count]
        ];
    }

    protected List<Member> GetSenpaiItems()
    {
        if ( _senpais.Count == 0 )
        {
            return [];
        }

        int count = _senpais.Count;
        return
        [
            _senpais[( _senpaiIndex - 1 + count ) % count],
            _senpais[_senpaiIndex % count],
            _senpais[( _senpaiIndex + 1 ) % count],
            _senpais[( _senpaiIndex + 2 ) % count],
            _senpais[( _senpaiIndex + 3 ) % count]
        ];
    }

    protected void PauseAutoAnimation()
    {
        _userInteractionPaused = true;
        _timer?.Stop();

        _resumeTimer?.Stop();
        _resumeTimer?.Dispose();
        _resumeTimer = new System.Timers.Timer(15_000);
        _resumeTimer.Elapsed  += OnResumeTimerElapsed;
        _resumeTimer.AutoReset = false;
        _resumeTimer.Enabled   = true;
    }

    protected void OnResumeTimerElapsed(object? sender, ElapsedEventArgs e)
    {
        _userInteractionPaused = false;

        if ( !_isAnimating )
        {
            _timer?.Start();
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

        if ( _isAnimating )
        {
            return;
        }

        _isAnimating = true;

        if ( _module is not null )
        {
            await _module.InvokeAsync<object>(slidesLeft ? "slideTrackLeft" : "slideTrackRight", CompetitorGridId);
        }

        if ( slidesLeft )
        {
            NextCompetitor();
        }
        else
        {
            PrevCompetitor();
        }

        _trackResetPending = true;
        await InvokeAsync(StateHasChanged);
    }

    protected async Task PerformSenpaiSlide(bool slidesLeft)
    {
        PauseAutoAnimation();

        if ( _isAnimating )
        {
            return;
        }

        _isAnimating = true;

        if ( _module is not null )
        {
            await _module.InvokeAsync<object>(slidesLeft ? "slideTrackLeft" : "slideTrackRight", SenpaiGridId);
        }

        if ( slidesLeft )
        {
            NextSenpai();
        }
        else
        {
            PrevSenpai();
        }

        _trackResetPending = true;
        await InvokeAsync(StateHasChanged);
    }

    protected void NextCompetitor()
    {
        if ( _competitors.Count > 0 )
        {
            _competitorIndex = ( _competitorIndex + 1 ) % _competitors.Count;
        }
    }

    protected void PrevCompetitor()
    {
        if ( _competitors.Count > 0 )
        {
            _competitorIndex = ( _competitorIndex - 1 + _competitors.Count ) % _competitors.Count;
        }
    }

    protected void NextSenpai()
    {
        if ( _senpais.Count > 0 )
        {
            _senpaiIndex = ( _senpaiIndex + 1 ) % _senpais.Count;
        }
    }

    protected void PrevSenpai()
    {
        if ( _senpais.Count > 0 )
        {
            _senpaiIndex = ( _senpaiIndex - 1 + _senpais.Count ) % _senpais.Count;
        }
    }

    protected Task OpenCompetitorResultsDialogAsync(Member member) =>
        DialogService.OpenAsync<CompetitorResultsDialog>(
            $"{member.Name}{L["Achievements"]}",
            new Dictionary<string, object?> { { "Member", member } },
            DialogDefaults.Options());

    protected Task OpenSenpaiResultsDialogAsync(Member member) =>
        DialogService.OpenAsync<SenpaiResultsDialog>(
            $"{member.Name}{L["Achievements"]}",
            new Dictionary<string, object?> { { "Member", member } },
            DialogDefaults.Options());
}
