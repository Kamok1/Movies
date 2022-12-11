namespace Auth.Abstractions
{
    public interface IAuthServices
    {
        string GetToken();
        bool ValidateToken();
    }
}