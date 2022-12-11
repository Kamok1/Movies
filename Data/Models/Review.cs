using System.ComponentModel.DataAnnotations;

namespace Data.Models;

public record Review()
{
    [Key]
    public int Id { get; set; }
    [Required]
    [MaxLength(100)]
    public string Title { get; set; }
    [Required]
    [MaxLength(1000)]
    public string Body { get; set; }
    public DateTime Created { get; set; }
    public Movie Movie { get; set; }
    public User User { get; set; }
    public float Rate { get; set; }

};