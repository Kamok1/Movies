using System.ComponentModel.DataAnnotations;

namespace Data.Models;

public record Genre
{
    [Key]
    public int Id { get; set; }
    [Required]
    [MaxLength(100)]
    public string Name { get; set; }
    public virtual ICollection<Movie> Movies { get; set; }
    
    public Genre(){}
    public Genre(string name) => Name = name;
}