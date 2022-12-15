using System.Net;

namespace Models.Exceptions;

public class AuthorizationException : CustomException
{
    public AuthorizationException() : base(HttpStatusCode.Forbidden, $"Authorization failed")
    { }
}
