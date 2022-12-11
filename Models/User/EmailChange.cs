using System.ComponentModel.DataAnnotations;

namespace Models.User;

public record EmailChange
{
    [Required]
    [EmailAddress]
    public string Email { get; init; }
}