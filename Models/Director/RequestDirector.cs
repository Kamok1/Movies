namespace Models.Director;

public record RequestDirector
{
    public string Name { get; init; }
    public string Description { get; init; }
    public DateTime DateOfBirth { get; init; }

}