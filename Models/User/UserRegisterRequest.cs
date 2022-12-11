using System.ComponentModel.DataAnnotations;

namespace Models.User;

public record UserRegisterRequest
{
    [Required]
    public string Name { get; init; }
    [Required]
    [MinLength(5)]
    public string Password { get; init; }
    [Required]
    [EmailAddress]
    public string Email { get; init; }
}
