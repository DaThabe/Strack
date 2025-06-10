namespace IGPSport.Exceptions;

public class IGSportException : System.Exception
{
    public IGSportException(string message) : base(message)
    {
    }
    public IGSportException(string message, System.Exception innerException) : base(message, innerException)
    {
    }
    public IGSportException() : base()
    {

    }
}

