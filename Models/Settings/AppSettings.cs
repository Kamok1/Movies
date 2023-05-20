namespace Models.Settings;

public record AppSettings()
{
    public FileSettings File { get; set; }
    public int PageSize { get; init; }
    public ApiSettings Api { get; init; }
}