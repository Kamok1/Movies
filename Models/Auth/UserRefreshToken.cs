namespace Models.Auth;

public record UserRefreshToken
{
  public string Token { get; set; }
  public DateTime Expires { get; set; }
}