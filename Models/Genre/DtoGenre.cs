namespace Models.Genre;

public record DtoGenre
{
    public int Id { get; init; }
    public string Name { get; init; }

    public DtoGenre(Data.Models.Genre genre)
    {
        Name = genre.Name;
        Id = genre.Id;
    }
}