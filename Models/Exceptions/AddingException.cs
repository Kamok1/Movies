using System.Net;

namespace Models.Exceptions;

public class AddingException<T> : CustomException
{
    public AddingException(string? messege = null) : base(HttpStatusCode.Conflict, messege ?? $"{typeof(T).Name} Can not be added")
    { }
}
