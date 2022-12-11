using System.Net;

namespace Models.Exceptions;

public class DeletingException<T> : CustomException
{
    public DeletingException() : base(HttpStatusCode.Conflict, $"{typeof(T).Name} Can not be deleted")
    { }
}