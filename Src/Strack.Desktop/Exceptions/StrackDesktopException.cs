namespace Strack.Desktop.Exceptions;



internal class StrackDesktopException : Exception
{
    public StrackDesktopException()
    {
    }

    public StrackDesktopException(string? message) : base(message)
    {
    }

    public StrackDesktopException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
