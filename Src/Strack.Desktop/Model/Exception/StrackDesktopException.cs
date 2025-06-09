namespace Strack.Desktop.Model.Exception;



internal class StrackDesktopException : System.Exception
{
    public StrackDesktopException()
    {
    }

    public StrackDesktopException(string? message) : base(message)
    {
    }

    public StrackDesktopException(string? message, System.Exception? innerException) : base(message, innerException)
    {
    }
}
