namespace Strack.Exceptions;

public class StrackDbException : Exception
{
    public StrackDbException(string message) : base(message)
    {
    }
    public StrackDbException(string message, Exception innerException) : base(message, innerException)
    {
    }
    public StrackDbException() : base()
    {

    }
}

