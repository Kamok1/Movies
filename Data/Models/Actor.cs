using System.ComponentModel.DataAnnotations;

namespace Data.Models;

public record Actor()
{
    [Key] public int Id { get; set; }
    [Required][MaxLength(100)] public string Name { get; set; }
    [MaxLength(10000)] public string Description { get; set; }
    public DateTime DateOfBirth { get; set; }
    public virtual ICollection<Movie> Movies { get; set; }
}
