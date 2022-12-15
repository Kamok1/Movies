using System.Net;

namespace Models.Exceptions;

public class EditingException<T> : CustomException
{
    public EditingException() : base(HttpStatusCode.Conflict, $"{typeof(T).Name} Can not be edited")
    { }
}

