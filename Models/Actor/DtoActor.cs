namespace Models.Actor;

public record DtoActor
{
    public int Id { get; init; }
    public string Name { get; init; }
    public string DateOfBirth { get; init; }
    public string PhotoPath { get; init; }

    public DtoActor(Data.Models.Actor actor)
    {
        Name = actor.Name; 
        DateOfBirth = actor.DateOfBirth.ToShortDateString();
        Id = actor.Id;
        PhotoPath = actor.PhotoPath;
    }
}