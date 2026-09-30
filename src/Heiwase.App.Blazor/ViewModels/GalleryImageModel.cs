namespace Heiwase.App.Blazor.ViewModels;

/// <summary>
/// Represents a single gallery picture. For non-placeholder images, <see cref="Source"/> is the
/// canonical jpg URL and the avif/webp variants are derived from it via <see cref="Path.ChangeExtension(string, string)"/>.
/// For placeholder images, <see cref="Source"/> is used directly as the image source.
/// </summary>
public sealed record GalleryImageModel(string Source, string GridItemClass);