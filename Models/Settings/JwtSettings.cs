namespace Models.Settings;

public record JwtSettings()
{
    public string Issuer { get; init; }
    public string Audience { get; init; }
    public string Key { get; init; }
    public int Expire { get; init; }
    public int RefreshTokenExpire { get; init; }

}