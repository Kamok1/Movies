using System.ComponentModel.DataAnnotations;

namespace Data.Models;

public record User
{
    [Key]
    public int Id { get; set; }
    [Required]
    [MaxLength(100)]
    public string Login { get; set; }
    public string DisplayName { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public string PasswordSalt { get; set; }
    public string Description { get; set; }
    public Role Role { get; set; }
    public virtual ICollection<Movie> UserFavouriteMovies { get; set; } = new List<Movie>();
    public IEnumerable<Review> Reviews { get; set; }
}