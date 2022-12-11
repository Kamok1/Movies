using System.ComponentModel.DataAnnotations;

namespace Data.Models;
public record Picture()
{
    [Required]
    public Movie Movie { get; set; }
    [Required]
    [Key]
    public string Path { get; set; }
}