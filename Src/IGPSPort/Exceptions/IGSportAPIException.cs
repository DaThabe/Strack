namespace IGPSport.Exceptions;

public class IGSportAPIException : IGSportException
{
    public IGSportAPIException(string message) : base(message)
    {
    }
    public IGSportAPIException(string message, System.Exception innerException) : base(message, innerException)
    {
    }
    public IGSportAPIException() : base()
    {

    }
}

