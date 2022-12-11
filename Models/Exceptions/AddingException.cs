using System.Net;

namespace Models.Exceptions;

public class AddingException<T> : CustomException
{
    public AddingException() : base(HttpStatusCode.Conflict, $"{typeof(T).Name} Can not be added")
    { }
}
