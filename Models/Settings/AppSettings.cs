namespace Models.Settings;

public record AppSettings()
{
    public string PicturesPath { get; init; }
    public string ResourcesPath { get; init; }
    public string PostersPath { get; init; }
    public int PageSize { get; init; }
    public ApiSettings Api { get; init; }
}