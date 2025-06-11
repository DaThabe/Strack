namespace Strack.Exceptions;

public class StrackException : Exception
{
    public StrackException(string message) : base(message)
    {
    }
    public StrackException(string message, Exception innerException) : base(message, innerException)
    {
    }
    public StrackException() : base()
    {

    }
}

