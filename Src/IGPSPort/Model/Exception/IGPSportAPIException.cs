namespace XingZhe.Model.Exception;

public class IGPSportAPIException : IGPSportException
{
    public IGPSportAPIException(string message) : base(message)
    {
    }
    public IGPSportAPIException(string message, System.Exception innerException) : base(message, innerException)
    {
    }
    public IGPSportAPIException() : base()
    {

    }
}

