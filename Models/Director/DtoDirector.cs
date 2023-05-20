namespace Models.Director;

public record DtoDirector
{
    public string Name { get; init; }
    public string Description { get; init; }
    public int Id { get; init; }
    public string PhotoPath { get; init; }
    public DateTime DateOfBirth { get; init; }

    public DtoDirector(Data.Models.Director director)
    {
        Description = director.Description;
        Id = director.Id;
        Name = director.Name;
        DateOfBirth = director.DateOfBirth;
        PhotoPath = director.PhotoPath;
    }
}