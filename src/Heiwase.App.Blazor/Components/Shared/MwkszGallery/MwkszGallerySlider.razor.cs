using Heiwase.App.Blazor.ViewModels;

using Microsoft.AspNetCore.Components;

using System.Timers;

namespace Heiwase.App.Blazor.Components.Shared.MwkszGallery;

public partial class MwkszGallerySlider : IDisposable
{
    private const int PageCount = 3;
    private const int AutoSlideIntervalMilliseconds = 5000;
    private const int SlideAnimationDurationMilliseconds = 600;

    private const string MediaBaseUrl = "https://heiwasemedia.blob.core.windows.net/public-media/img/mwkszgrid";

    private static readonly IReadOnlyList<IReadOnlyList<GalleryImageModel>> _pages =
    [
        [
            new GalleryImageModel($"{MediaBaseUrl}/csoport.jpg", "g-item-1"),
            new GalleryImageModel($"{MediaBaseUrl}/katica.jpg", "g-item-2"),
            new GalleryImageModel($"{MediaBaseUrl}/technika.jpg", "g-item-3"),
            new GalleryImageModel($"{MediaBaseUrl}/verseny.jpg", "g-item-4"),
        ],
        [
            new GalleryImageModel($"{MediaBaseUrl}/biro.jpg", "g-item-5"),
            new GalleryImageModel($"{MediaBaseUrl}/vb.jpg", "g-item-6"),
            new GalleryImageModel($"{MediaBaseUrl}/kupa.jpg", "g-item-7"),
            new GalleryImageModel($"{MediaBaseUrl}/opera_seminar.jpg", "g-item-8"),
        ],
        [
            new GalleryImageModel($"{MediaBaseUrl}/sem_eletkep.jpg", "g-item-9"),
            new GalleryImageModel($"{MediaBaseUrl}/opera_csoportkep.jpg", "g-item-10"),
            new GalleryImageModel($"{MediaBaseUrl}/akos_kata.jpg", "g-item-11"),
            new GalleryImageModel($"{MediaBaseUrl}/verseny_rugas.jpg", "g-item-12"),
        ],
    ];

    [Parameter]
    public string ImageAltText { get; set; } = "Gallery photo";

    [Parameter]
    public string PreviousButtonAriaLabel { get; set; } = "Previous";

    [Parameter]
    public string NextButtonAriaLabel { get; set; } = "Next";

    private readonly System.Timers.Timer _autoSlideTimer = new(AutoSlideIntervalMilliseconds) { AutoReset = true };

    private int _currentPage;
    private int? _previousPage;
    private SlideDirection _direction = SlideDirection.Next;
    private bool _isAnimating;

    protected override void OnInitialized()
    {
        _autoSlideTimer.Elapsed += OnAutoSlideElapsed;
        _autoSlideTimer.Start();
    }

    private async void OnAutoSlideElapsed(object? sender, ElapsedEventArgs e)
        => await InvokeAsync(() => SlideAsync(SlideDirection.Next));

    private Task OnPreviousClick()
        => SlideAsync(SlideDirection.Previous);

    private Task OnNextClick()
        => SlideAsync(SlideDirection.Next);

    private async Task SlideAsync(SlideDirection direction)
    {
        if (_isAnimating)
        {
            return;
        }

        // Pause the auto-slide timer; it is restarted (reset) once the requested transition completes.
        _autoSlideTimer.Stop();

        _direction = direction;
        _previousPage = _currentPage;
        _currentPage = direction == SlideDirection.Next
            ? (_currentPage + 1) % PageCount
            : (_currentPage - 1 + PageCount) % PageCount;
        _isAnimating = true;
        StateHasChanged();

        await Task.Delay(SlideAnimationDurationMilliseconds);

        _previousPage = null;
        _isAnimating = false;
        StateHasChanged();

        _autoSlideTimer.Start();
    }

    private string GetExitAnimationCssClass()
        => _direction == SlideDirection.Next ? "gallery-slide-out-left" : "gallery-slide-out-right";

    private string GetEnterAnimationCssClass()
        => _direction == SlideDirection.Next ? "gallery-slide-in-right" : "gallery-slide-in-left";

    private static string GetCollageKey(int pageIndex, bool isExiting)
        => $"gallery-page-{pageIndex}-{(isExiting ? "exit" : "enter")}";

    public void Dispose()
    {
        _autoSlideTimer.Elapsed -= OnAutoSlideElapsed;
        _autoSlideTimer.Dispose();
        GC.SuppressFinalize(this);
    }

    private enum SlideDirection
    {
        Next,
        Previous
    }
}
