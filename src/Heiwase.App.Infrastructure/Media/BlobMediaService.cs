using System.Text.RegularExpressions;

using Azure.Core;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

using Heiwase.App.Infrastructure.Configuration;
using Heiwase.App.Infrastructure.Interface;

using Microsoft.Extensions.Options;

namespace Heiwase.App.Infrastructure.Media;

internal sealed partial class BlobMediaService : IMediaService
{
    private const string CacheControl = "public, max-age=31536000, immutable";

    private static readonly Dictionary<string, string> Extensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ["image/jpeg"] = ".jpg",
        ["image/png"] = ".png",
        ["image/webp"] = ".webp",
        ["image/gif"] = ".gif",
    };

    private readonly BlobContainerClient _container;
    private readonly int _maxFileSize;

    public BlobMediaService(IOptions<MediaOptions> options, TokenCredential credential)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(credential);

        MediaOptions settings = options.Value;
        string serviceUri = string.IsNullOrWhiteSpace(settings.BlobEndpoint)
            ? $"https://{settings.AccountName}.blob.core.windows.net"
            : settings.BlobEndpoint.TrimEnd('/');

        _container = new BlobContainerClient(new Uri($"{serviceUri}/{settings.ContainerName}"), credential);
        _maxFileSize = settings.MaxFileSizeBytes;
    }

    public async Task<string> UploadImageAsync(Stream content, string fileName, string contentType, string folder, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(content);
        ArgumentException.ThrowIfNullOrWhiteSpace(contentType);
        ArgumentException.ThrowIfNullOrWhiteSpace(folder);

        if (!content.CanRead)
        {
            throw new MediaValidationException("The file stream is not readable.");
        }

        if (!Extensions.TryGetValue(contentType, out string? extension))
        {
            throw new MediaValidationException($"Content type '{contentType}' is not allowed.");
        }

        if (!FolderPattern().IsMatch(folder))
        {
            throw new MediaValidationException("Folder name is invalid.");
        }

        MemoryStream? buffer = null;
        try
        {
            Stream source = content;
            if (!content.CanSeek)
            {
                buffer = await CopyWithLimitAsync(content, cancellationToken).ConfigureAwait(false);
                source = buffer;
            }
            else if (content.Length > _maxFileSize)
            {
                throw new MediaValidationException($"The file exceeds the {_maxFileSize} byte limit.");
            }

            if (source.Length == 0)
            {
                throw new MediaValidationException("The file is empty.");
            }

            await ValidateSignatureAsync(source, contentType, cancellationToken).ConfigureAwait(false);

            DateTime now = DateTime.UtcNow;
            string blobName = $"{folder}/{now:yyyy}/{now:MM}/{Guid.NewGuid():N}{extension}";
            BlobClient blob = _container.GetBlobClient(blobName);

            await blob.UploadAsync(
                source,
                new BlobUploadOptions
                {
                    HttpHeaders = new BlobHttpHeaders { ContentType = contentType, CacheControl = CacheControl },
                },
                cancellationToken).ConfigureAwait(false);

            return blob.Uri.ToString();
        }
        finally
        {
            if (buffer is not null)
            {
                await buffer.DisposeAsync().ConfigureAwait(false);
            }
        }
    }

    public async Task DeleteAsync(string blobUrl, CancellationToken cancellationToken = default)
    {
        if (!Uri.TryCreate(blobUrl, UriKind.Absolute, out Uri? uri))
        {
            return;
        }

        string containerPath = _container.Uri.AbsolutePath.TrimEnd('/') + "/";
        if (!string.Equals(uri.Scheme, _container.Uri.Scheme, StringComparison.OrdinalIgnoreCase)
            || uri.Port != _container.Uri.Port
            || !string.Equals(uri.Host, _container.Uri.Host, StringComparison.OrdinalIgnoreCase)
            || !uri.AbsolutePath.StartsWith(containerPath, StringComparison.Ordinal))
        {
            return;
        }

        string blobName = Uri.UnescapeDataString(uri.AbsolutePath[containerPath.Length..]);
        if (string.IsNullOrEmpty(blobName))
        {
            return;
        }

        await _container.GetBlobClient(blobName)
            .DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots, cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    private async Task<MemoryStream> CopyWithLimitAsync(Stream content, CancellationToken cancellationToken)
    {
        MemoryStream buffer = new();
        byte[] chunk = new byte[81920];
        int read;
        while ((read = await content.ReadAsync(chunk, cancellationToken).ConfigureAwait(false)) > 0)
        {
            if (buffer.Length + read > _maxFileSize)
            {
                await buffer.DisposeAsync().ConfigureAwait(false);
                throw new MediaValidationException($"The file exceeds the {_maxFileSize} byte limit.");
            }

            buffer.Write(chunk, 0, read);
        }

        buffer.Position = 0;
        return buffer;
    }

    private static async Task ValidateSignatureAsync(Stream stream, string contentType, CancellationToken cancellationToken)
    {
        long start = stream.Position;
        byte[] header = new byte[12];
        int total = 0;
        int read;
        while (total < header.Length
            && (read = await stream.ReadAsync(header.AsMemory(total), cancellationToken).ConfigureAwait(false)) > 0)
        {
            total += read;
        }

        stream.Position = start;

        ReadOnlySpan<byte> bytes = header.AsSpan(0, total);
        bool valid = contentType.ToLowerInvariant() switch
        {
            "image/jpeg" => bytes.StartsWith(new byte[] { 0xFF, 0xD8, 0xFF }),
            "image/png" => bytes.StartsWith(new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A }),
            "image/gif" => bytes.StartsWith("GIF87a"u8) || bytes.StartsWith("GIF89a"u8),
            "image/webp" => bytes.Length >= 12 && bytes[..4].SequenceEqual("RIFF"u8) && bytes[8..12].SequenceEqual("WEBP"u8),
            _ => false,
        };

        if (!valid)
        {
            throw new MediaValidationException("The file content does not match the declared content type.");
        }
    }

    [GeneratedRegex("^[a-z0-9-]+(/[a-z0-9-]+)*$")]
    private static partial Regex FolderPattern();
}
