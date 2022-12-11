namespace Models.Movie;

public record RequestMovie
{
    public string Title { get; init; }
    public string Description { get; init; }
    public DateTime ReleaseDate { get; init; }
}