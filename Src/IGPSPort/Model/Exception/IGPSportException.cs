namespace XingZhe.Model.Exception;

public class IGPSportException : System.Exception
{
    public IGPSportException(string message) : base(message)
    {
    }
    public IGPSportException(string message, System.Exception innerException) : base(message, innerException)
    {
    }
    public IGPSportException() : base()
    {

    }
}

