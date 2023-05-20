namespace Models.User;

public record UserRequestRefreshToken
{
  public string Token { get; set; }
  public string Ip { get; set; }
}