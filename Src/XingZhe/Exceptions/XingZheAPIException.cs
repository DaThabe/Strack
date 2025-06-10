namespace XingZhe.Exceptions;

public class XingZheAPIException : XingZheException
{
    public XingZheAPIException(string message) : base(message)
    {
    }
    public XingZheAPIException(string message, System.Exception innerException) : base(message, innerException)
    {
    }
    public XingZheAPIException() : base()
    {

    }
}

