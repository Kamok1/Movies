using System.ComponentModel.DataAnnotations;

namespace Models.User;

public record EditUserProfile
{
    [MaxLength(100)]
    public string DisplayName { get; init; }
    public string Description { get; init; }
}
