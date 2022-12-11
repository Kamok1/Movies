using System.ComponentModel.DataAnnotations;

namespace Models.User;

public record PasswordChange
{
    [Required]
    public string OldPassword { get; init; }
    [Required]
    public string NewPassword { get; init; }
}