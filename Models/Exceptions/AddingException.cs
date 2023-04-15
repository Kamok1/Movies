using System.Net;

namespace Models.Exceptions;

public class AddingException<T> : CustomException
{
    public AddingException(string? message = null) : base(HttpStatusCode.Conflict, message ?? $"{typeof(T).Name} Can not be added")
    { }
}
