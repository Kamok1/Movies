namespace Models.Auth;

public record JwtResponse
{
  public string Jwt { get; init; }
  public int ExpiresIn { get; init; }

}