namespace Models.Genre;

public record GenresRequest
{
    public List<string> GenreNames { get; init; }
}