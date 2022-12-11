using System.Net;

namespace Models.Exceptions;


public class NotFoundException<T> : CustomException
{
    public NotFoundException() : base(HttpStatusCode.NotFound, $"{typeof(T).Name} Not Found")
    { }
}

