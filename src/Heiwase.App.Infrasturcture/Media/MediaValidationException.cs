namespace Heiwase.App.Infrastructure.Media;

public sealed class MediaValidationException : Exception
{
    public MediaValidationException(string message)
        : base(message)
    {
    }
}
