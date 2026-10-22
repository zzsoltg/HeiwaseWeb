namespace Heiwase.App.Infrastructure.Interface;

public interface IMediaService
{
    /// <summary>
    /// Uploads an image to the public media container and returns its public HTTPS URL.
    /// </summary>
    /// <param name="folder">Logical prefix, e.g. <see cref="Media.MediaFolders.Articles"/>.</param>
    /// <exception cref="Media.MediaValidationException">The content type, size, content or folder is invalid.</exception>
    Task<string> UploadImageAsync(Stream content, string fileName, string contentType, string folder, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a blob by its public URL. Does nothing for foreign URLs or missing blobs.
    /// </summary>
    Task DeleteAsync(string blobUrl, CancellationToken cancellationToken = default);
}
