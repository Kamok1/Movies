namespace Models.Actor;

public record EditMovieActors
{
    public List<int> ActorsId { get; init; }
    public int MovieId { get; init; }
}