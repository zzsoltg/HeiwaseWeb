using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

using System.Net.Http.Json;
using System.Timers;

namespace Heiwase.App.Blazor.Pages.Home;

public partial class Home : IAsyncDisposable
{
    [Inject]
    public IJSRuntime JS { get; set; } = default!;

    [Inject]
    public HttpClient Http { get; set; } = default!;

    private IJSObjectReference? _module;
    private bool _isMenuOpen = false;
    private ApplicantModel _applicant = new();
    private bool _formSubmitted = false;
    private List<Member> _competitors = [];
    private List<Member> _senpais = [];
    private int _competitorIndex = 0;
    private int _senpaiIndex = 0;
    private System.Timers.Timer? _timer;
    private bool _trackInitialized = false;
    private bool _trackResetPending = false;
    private bool _isAnimating = false;
    private bool _userInteractionPaused = false;

    private const string HallOfFameDataString = "data/halloffame.json";
    private const string CompetitorGridId = "competitors-grid";
    private const string SenpaiGridId = "senpais-grid";

    private System.Timers.Timer? _resumeTimer;

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

    private void ToggleMenu() => _isMenuOpen = !_isMenuOpen;

    private void CloseMenu() => _isMenuOpen = false;

    private void HandleValidSubmit()
    {
        _formSubmitted = true;
        _applicant = new ApplicantModel();
    }

    private void StartTimer()
    {
        _timer = new System.Timers.Timer(3000);
        _timer.Elapsed += OnTimerElapsed;
        _timer.AutoReset = true;
        _timer.Enabled = true;
    }

    private async void OnTimerElapsed(object? sender, ElapsedEventArgs e)
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

    // Returns 5 items: buffer-left + 3 visible + buffer-right.
    // Resting track position (-slotWidth) shows cards 1–3.
    // slideTrackRight: shows cards 0–2 (buffer enters left, card 4 exits right).
    // slideTrackLeft:  shows cards 2–4 (card 0 exits left, buffer enters right).
    private List<Member> GetCompetitorItems()
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

    // Returns 5 items: buffer-left + 3 visible + buffer-right.
    private List<Member> GetSenpaiItems()
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

    // Stops the auto-animation timer and starts/resets the 15-second resume countdown.
    // Call on every button press so user interaction always postpones auto-animation.
    private void PauseAutoAnimation()
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

    private void OnResumeTimerElapsed(object? sender, ElapsedEventArgs e)
    {
        _userInteractionPaused = false;

        // If no animation is currently running, restart the auto-animation timer.
        // If one is running, OnAfterRenderAsync will restart it once the animation finishes.
        if ( !_isAnimating )
        {
            _timer?.Start();
        }
    }

    private Task OnCompetitorLeftClick()  => PerformCompetitorSlide(slidesLeft: true);
    private Task OnCompetitorRightClick() => PerformCompetitorSlide(slidesLeft: false);
    private Task OnSenpaiLeftClick()      => PerformSenpaiSlide(slidesLeft: true);
    private Task OnSenpaiRightClick()     => PerformSenpaiSlide(slidesLeft: false);

    private async Task PerformCompetitorSlide(bool slidesLeft)
    {
        PauseAutoAnimation();

        if ( _isAnimating ) return;
        _isAnimating = true;

        if ( _module is not null )
        {
            await _module.InvokeAsync<object>(slidesLeft ? "slideTrackLeft" : "slideTrackRight", CompetitorGridId);
        }

        if ( slidesLeft ) NextCompetitor();
        else PrevCompetitor();

        _trackResetPending = true;
        await InvokeAsync(StateHasChanged);
    }

    private async Task PerformSenpaiSlide(bool slidesLeft)
    {
        PauseAutoAnimation();

        if ( _isAnimating ) return;
        _isAnimating = true;

        if ( _module is not null )
        {
            await _module.InvokeAsync<object>(slidesLeft ? "slideTrackLeft" : "slideTrackRight", SenpaiGridId);
        }

        if ( slidesLeft ) NextSenpai();
        else PrevSenpai();

        _trackResetPending = true;
        await InvokeAsync(StateHasChanged);
    }

    private void NextCompetitor()
    {
        if ( _competitors.Count > 0 )
        {
            _competitorIndex = ( _competitorIndex + 1 ) % _competitors.Count;
        }
    }

    private void PrevCompetitor()
    {
        if ( _competitors.Count > 0 )
        {
            _competitorIndex = ( _competitorIndex - 1 + _competitors.Count ) % _competitors.Count;
        }
    }

    private void NextSenpai()
    {
        if ( _senpais.Count > 0 )
        {
            _senpaiIndex = ( _senpaiIndex + 1 ) % _senpais.Count;
        }
    }

    private void PrevSenpai()
    {
        if ( _senpais.Count > 0 )
        {
            _senpaiIndex = ( _senpaiIndex - 1 + _senpais.Count ) % _senpais.Count;
        }
    }
}
