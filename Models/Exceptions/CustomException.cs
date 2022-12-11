using System.Net;

namespace Models.Exceptions;

public abstract class CustomException : Exception
{
    public HttpStatusCode StatusCode;
    public new string Message;

    protected CustomException(HttpStatusCode statusCode, string message)
    {
        StatusCode = statusCode;
        Message = message;
    }
};