namespace Models.Movie;

public record DtoMovie()
{
    public List<string> Genres { get; init; }
    public string Title { get; init; }
    public float Rating { get; init; }
    public string Description { get; init; }
    public string? Director { get; init; }
    public DateTime ReleaseDate { get; init; }
    public string PosterPath { get; init; }
    public int Id { get; init; }

    public DtoMovie(Data.Models.Movie movie) : this()
    {
        Id = movie.Id;
        Title = movie.Title;
        Rating = movie.Reviews.Any() ? movie.Reviews.Average(x => x.Rate) : 0;
        Description = movie.Description;
        Director = movie.Director?.Name;
        PosterPath = movie.Posters.FirstOrDefault(poster => poster.IsMain)?.Path ?? "poster/empty.jpg";
        Genres = movie.Genres.Select(x => x.Name).ToList();
        ReleaseDate = movie.ReleaseDate;
    }
}