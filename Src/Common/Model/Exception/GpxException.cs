namespace Common.Model.Exception;

public class GpxException : System.Exception
{
    public GpxException()
    {
    }

    public GpxException(string? message) : base(message)
    {
    }

    public GpxException(string? message, System.Exception? innerException) : base(message, innerException)
    {
    }
}
