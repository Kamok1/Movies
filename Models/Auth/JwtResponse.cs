namespace Models.Auth;

public record JwtResponse
{
  public string Token { get; set; }
  public UserRefreshToken RefreshToken { get; set; }
}