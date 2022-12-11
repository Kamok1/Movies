namespace Models.Director;

public record EditMovieDirector
{
    public int DirectorId { get; init; }
    public int MovieId { get; init; }
}