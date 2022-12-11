using Auth.Abstractions;

namespace Auth
{
    public class AuthServices : IAuthServices
    {
        string GetToken();
        bool ValidateToken();
    }
}