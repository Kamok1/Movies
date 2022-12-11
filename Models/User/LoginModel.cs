using System.ComponentModel.DataAnnotations;

namespace Models.User;
public record LoginModel
{
    [Required]
    public string Login { get; init; }
    [Required]
    public string Password { get; init; }
};