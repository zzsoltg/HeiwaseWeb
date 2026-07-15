using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

using System.Net.Http.Json;
using System.Timers;

namespace HeiwaseWeb2.Pages;

public partial class Home : IDisposable
{
    [Inject]
    public required IJSRuntime JS { get; set; }

    [Inject]
    public required HttpClient Http { get; set; }

    private bool _isMenuOpen = false;
    private ApplicantModel _applicant = new();
    private bool _formSubmitted = false;
    private List<Member> _competitors = [];
    private List<Member> _senpais = [];
    private int _competitorIndex = 0;
    private int _senpaiIndex = 0;
    private System.Timers.Timer? _timer;

    private const string HallOfFameDataString = "data/halloffame.json";

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

            StartTimer();
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

    private void OnTimerElapsed(object? sender, ElapsedEventArgs e)
    {
        NextCompetitor();
        NextSenpai();
        InvokeAsync(StateHasChanged);
    }

    public void Dispose()
    {
        _timer?.Dispose();
        GC.SuppressFinalize(this);
    }

    private static List<Member> GetVisibleItems(List<Member> list, int index)
    {
        var result = new List<Member>();
        if ( list == null || list.Count == 0 )
        {
            return result;
        }

        for ( int i = 0; i < 3; i++ )
        {
            result.Add(list[( index + i ) % list.Count]);
        }

        return result;
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
