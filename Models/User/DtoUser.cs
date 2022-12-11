namespace Models.User;

public record DtoUser
{
    public int Id { get; init; }
    public string DisplayName { get; init; }
    public string Description { get; init; }
    public DtoUser(Data.Models.User user)
    {
        Id = user.Id;
        DisplayName = user.DisplayName;
        Description = user.Description;
    }
}