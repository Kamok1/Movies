namespace Models.Settings;

public record ApiSettings()
{
    public string Url { get; init; }
    public string Key { get; init; }
    public string Host { get; init; }
}