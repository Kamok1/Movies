using System.ComponentModel.DataAnnotations;

namespace Models.Actor;

public record RequestActor
{
    [Required]
    public string Name { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Description { get; set; }
}
