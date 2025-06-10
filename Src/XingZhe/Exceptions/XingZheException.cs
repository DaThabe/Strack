namespace XingZhe.Exceptions;

public class XingZheException : System.Exception
{
    public XingZheException(string message) : base(message)
    {
    }
    public XingZheException(string message, System.Exception innerException) : base(message, innerException)
    {
    }
    public XingZheException() : base()
    {

    }
}

