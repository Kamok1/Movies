namespace Models.Auth;

public record RefreshTokenRequest()
{
  public string Token { get; init; }
}