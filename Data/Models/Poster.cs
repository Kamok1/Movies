using System.ComponentModel.DataAnnotations;

namespace Data.Models;

public record Poster()
{
    [Required]
    public Movie Movie { get; set; }
    [Required]
    [Key]
    public string Path { get; set; }
    public bool IsMain { get; set; } = false;
}