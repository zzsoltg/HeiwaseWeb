using Azure.Core;

using Heiwase.App.Infrastructure.Configuration;
using Heiwase.App.Infrastructure.Media;

using Microsoft.Extensions.Options;

using NSubstitute;

namespace Heiwase.App.Tests.Unit.Infrastructure;

public sealed class BlobMediaServiceTests
{
    private static readonly byte[] Jpeg = [0xFF, 0xD8, 0xFF, 0xE0, 0, 0, 0, 0, 0, 0, 0, 0];

    private static BlobMediaService CreateSut(int maxFileSize = 1024)
    {
        var options = Options.Create(new MediaOptions
        {
            AccountName = "acct",
            ContainerName = "public-media",
            BlobEndpoint = "https://blob.example.test/",
            MaxFileSizeBytes = maxFileSize,
        });

        return new BlobMediaService(options, Substitute.For<TokenCredential>());
    }

    /// <summary>Stream that reports CanSeek = false to exercise the buffered path.</summary>
    private sealed class NonSeekableStream(byte[] data) : MemoryStream(data)
    {
        public override bool CanSeek => false;
    }

    private static Task<string> Upload(BlobMediaService sut, Stream stream, string contentType = "image/jpeg", string folder = "articles")
        => sut.UploadImageAsync(stream, "file", contentType, folder);

    [Fact]
    public void Constructor_Rejects_Null_Arguments()
    {
        Assert.Throws<ArgumentNullException>(() => new BlobMediaService(null!, Substitute.For<TokenCredential>()));
        Assert.Throws<ArgumentNullException>(() => new BlobMediaService(Options.Create(new MediaOptions()), null!));
    }

    [Fact]
    public void Constructor_Builds_Default_Endpoint_From_Account_Name()
    {
        var options = Options.Create(new MediaOptions { AccountName = "acct" });

        BlobMediaService sut = new(options, Substitute.For<TokenCredential>());

        Assert.NotNull(sut);
    }

    [Fact]
    public async Task Upload_Rejects_Null_Stream()
        => await Assert.ThrowsAsync<ArgumentNullException>(() => Upload(CreateSut(), null!));

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public async Task Upload_Rejects_Blank_ContentType(string contentType)
    {
        using MemoryStream stream = new(Jpeg);
        await Assert.ThrowsAsync<ArgumentException>(() => Upload(CreateSut(), stream, contentType));
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public async Task Upload_Rejects_Blank_Folder(string folder)
    {
        using MemoryStream stream = new(Jpeg);
        await Assert.ThrowsAsync<ArgumentException>(() => Upload(CreateSut(), stream, folder: folder));
    }

    [Fact]
    public async Task Upload_Rejects_Unreadable_Stream()
    {
        MemoryStream stream = new(Jpeg);
        stream.Close();

        var ex = await Assert.ThrowsAsync<MediaValidationException>(() => Upload(CreateSut(), stream));

        Assert.Contains("not readable", ex.Message);
    }

    [Theory]
    [InlineData("image/svg+xml")]
    [InlineData("application/pdf")]
    [InlineData("text/html")]
    public async Task Upload_Rejects_Disallowed_Content_Types(string contentType)
    {
        using MemoryStream stream = new(Jpeg);

        var ex = await Assert.ThrowsAsync<MediaValidationException>(
            () => Upload(CreateSut(), stream, contentType));

        Assert.Contains("not allowed", ex.Message);
    }

    [Theory]
    [InlineData("../secret")]
    [InlineData("Articles")]
    [InlineData("a b")]
    [InlineData("/articles")]
    [InlineData("articles/")]
    [InlineData("articles//x")]
    public async Task Upload_Rejects_Invalid_Folder_Names(string folder)
    {
        var ex = await Assert.ThrowsAsync<MediaValidationException>(
            () => Upload(CreateSut(), new MemoryStream(Jpeg), folder: folder));

        Assert.Contains("Folder", ex.Message);
    }

    [Fact]
    public async Task Upload_Rejects_Oversized_Seekable_Stream()
    {
        using MemoryStream stream = new(Jpeg);
        var ex = await Assert.ThrowsAsync<MediaValidationException>(
            () => Upload(CreateSut(maxFileSize: 8), stream));

        Assert.Contains("limit", ex.Message);
    }

    [Fact]
    public async Task Upload_Rejects_Oversized_NonSeekable_Stream()
    {
        var ex = await Assert.ThrowsAsync<MediaValidationException>(
            () => Upload(CreateSut(maxFileSize: 8), new NonSeekableStream(Jpeg)));

        Assert.Contains("limit", ex.Message);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task Upload_Rejects_Empty_Files(bool seekable)
    {
        using Stream stream = seekable ? new MemoryStream([]) : new NonSeekableStream([]);

        var ex = await Assert.ThrowsAsync<MediaValidationException>(() => Upload(CreateSut(), stream));

        Assert.Contains("empty", ex.Message);
    }

    [Theory]
    [InlineData("image/jpeg", new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A, 0, 0, 0, 0 })]
    [InlineData("image/png", new byte[] { 0xFF, 0xD8, 0xFF, 0xE0, 0, 0, 0, 0, 0, 0, 0, 0 })]
    [InlineData("image/gif", new byte[] { 0xFF, 0xD8, 0xFF, 0xE0, 0, 0, 0, 0, 0, 0, 0, 0 })]
    [InlineData("image/webp", new byte[] { 0xFF, 0xD8, 0xFF, 0xE0, 0, 0, 0, 0, 0, 0, 0, 0 })]
    [InlineData("image/webp", new byte[] { (byte) 'R', (byte) 'I', (byte) 'F', (byte) 'F', 0, 0, 0, 0, 0, 0, 0, 0 })]
    [InlineData("image/jpeg", new byte[] { 0xFF })]
    public async Task Upload_Rejects_Content_That_Does_Not_Match_The_Declared_Type(string contentType, byte[] data)
    {
        var ex = await Assert.ThrowsAsync<MediaValidationException>(
            () => Upload(CreateSut(), new MemoryStream(data), contentType));

        Assert.Contains("does not match", ex.Message);
    }

    [Fact]
    public async Task Upload_Content_Type_Check_Is_Case_Insensitive_And_Reaches_Signature_Validation()
    {
        using MemoryStream stream = new([1, 2, 3]);

        var ex = await Assert.ThrowsAsync<MediaValidationException>(
            () => Upload(CreateSut(), stream, "IMAGE/PNG"));

        Assert.Contains("does not match", ex.Message);
    }

    [Theory]
    [InlineData("not a url")]
    [InlineData("")]
    [InlineData("https://other.example.test/public-media/articles/a.jpg")]
    [InlineData("http://blob.example.test/public-media/articles/a.jpg")]
    [InlineData("https://blob.example.test:8443/public-media/articles/a.jpg")]
    [InlineData("https://blob.example.test/other-container/a.jpg")]
    [InlineData("https://blob.example.test/public-media-evil/a.jpg")]
    [InlineData("https://blob.example.test/public-media/")]
    public async Task Delete_Ignores_Foreign_Or_Malformed_Urls_Without_Touching_Storage(string url)
    {
        var sut = CreateSut();

        var ex = await Record.ExceptionAsync(() => sut.DeleteAsync(url));

        Assert.Null(ex);
    }

    [Fact]
    public void Media_Folders_Are_Valid_Upload_Folder_Names()
    {
        Assert.Equal("articles", MediaFolders.Articles);
        Assert.Equal("hall-of-fame", MediaFolders.HallOfFame);
    }

    [Fact]
    public void ValidationException_Carries_Message()
        => Assert.Equal("boom", new MediaValidationException("boom").Message);
}
