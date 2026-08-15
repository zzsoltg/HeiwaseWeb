using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

using Heiwase.App.Blazor.Components.Shared;

using Radzen;

using System.Net.Http.Json;
using System.Timers;

namespace Heiwase.App.Blazor.Components.Pages.HallOfFameSection;

public partial class HallOfFameSection : IAsyncDisposable
{
    [Inject]
    public IJSRuntime JS { get; set; } = default!;

    [Inject]
    public HttpClient Http { get; set; } = default!;

    [Inject]
    public DialogService DialogService { get; set; } = default!;

    private IJSObjectReference? _module;
    private List<Member> _competitors = [];
    private List<Member> _senpais = [];
    private System.Timers.Timer? _timer;
    private System.Timers.Timer? _resumeTimer;

    private bool _trackInitialized = false;
    private bool _trackResetPending = false;
    private bool _isAnimating = false;
    private bool _userInteractionPaused = false;
    private int _competitorIndex = 0;
    private int _senpaiIndex = 0;

    private const string HallOfFameDataString = "data/halloffame.json";
    private const string CompetitorGridId = "competitors-grid";
    private const string SenpaiGridId = "senpais-grid";

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

        if ( !_isAnimating )
        {
            _timer?.Start();
        }
    }

    private Task OnCompetitorLeftClick() 
        => PerformCompetitorSlide(slidesLeft: true);

    private Task OnCompetitorRightClick()
        => PerformCompetitorSlide(slidesLeft: false);

    private Task OnSenpaiLeftClick()
        => PerformSenpaiSlide(slidesLeft: true);

    private Task OnSenpaiRightClick()
        => PerformSenpaiSlide(slidesLeft: false);

    private async Task PerformCompetitorSlide(bool slidesLeft)
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

    private async Task PerformSenpaiSlide(bool slidesLeft)
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

    private Task OpenCompetitorResultsDialogAsync(Member member) =>
        DialogService.OpenAsync<CompetitorResultsDialog>($"{member.Name} eredményei");

    private Task OpenSenpaiResultsDialogAsync(Member member) =>
        DialogService.OpenAsync<SenpaiResultsDialog>($"{member.Name} eredményei");
}
