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

    private const string HallOfFameDataString = "data/halloffame.json";
    private const string CompetitorGridId = "competitors-grid";
    private const string SenpaiGridId = "senpais-grid";

    private bool _trackInitialized  = false;
    private bool _trackResetPending = false;

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
            await _module.InvokeVoidAsync("initCompetitorTrack", CompetitorGridId);
            await _module.InvokeVoidAsync("initSenpaiTrack", SenpaiGridId);
            StartTimer();
        }

        if ( _trackResetPending && _module is not null )
        {
            _trackResetPending = false;
            await _module.InvokeVoidAsync("resetCompetitorTrack", CompetitorGridId);
            await _module.InvokeVoidAsync("resetSenpaiTrack", SenpaiGridId);
            _timer?.Start();
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

        if ( _module is not null )
        {
            var compTask = _module.InvokeAsync<object>("slideCompetitorTrack", CompetitorGridId).AsTask();
            var senpTask = _module.InvokeAsync<object>("slideSenpaiTrack", SenpaiGridId).AsTask();
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
        GC.SuppressFinalize(this);

        if ( _module is not null )
        {
            await _module.DisposeAsync();
        }
    }

    // Returns 4 items: buffer (position 0, off-screen left) + 3 visible.
    // Top slider flows left→right; index decreases on each tick (PrevCompetitor).
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
            _competitors[( _competitorIndex + 2 ) % count]
        ];
    }

    // Returns 4 items: 3 visible + buffer (position 3, off-screen right).
    // Bottom slider flows right→left; index increases on each tick (NextSenpai).
    private List<Member> GetSenpaiItems()
    {
        if ( _senpais.Count == 0 )
        {
            return [];
        }

        int count = _senpais.Count;
        return
        [
            _senpais[_senpaiIndex % count],
            _senpais[( _senpaiIndex + 1 ) % count],
            _senpais[( _senpaiIndex + 2 ) % count],
            _senpais[( _senpaiIndex + 3 ) % count]
        ];
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
