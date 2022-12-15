namespace Models.Genre;

public record EditGenre
{
    public int MovieId { get; init; }
    public List<int> GenreIds { get; init; }

}