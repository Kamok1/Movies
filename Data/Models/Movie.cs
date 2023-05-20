using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models;

public class Movie
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(1000)]
    public string Title { get; set; }
    [MaxLength(100000)]
    public string Description { get; set; }
    [MaxLength(10000)]
    public string TrailerUrl { get; set; }

  [Column(TypeName = "timestamp with time zone")]
    public DateTime ReleaseDate { get; set; }
    public virtual ICollection<Poster> Posters { get; set; } = new List<Poster>();
    public virtual ICollection<Picture> Pictures { get; set; } = new List<Picture>();
    public virtual ICollection<Actor> Actors { get; set; } = new List<Actor>();
    public virtual ICollection<Genre> Genres { get; set; } = new List<Genre>();
    public virtual ICollection<User> UsersFavorite { get; set; } = new List<User>();
    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
    public virtual Director? Director { get; set; }

};