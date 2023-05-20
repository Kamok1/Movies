using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Movie> Movie { get; set; }
        public DbSet<Review> Review { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Genre> Genre { get; set; }
        public DbSet<Director> Director { get; set; }
        public DbSet<Actor> Actor { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<Picture> Picture { get; set; }
        public DbSet<Poster> Poster { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
          modelBuilder.Entity<Movie>().Navigation(movie => movie.Genres).AutoInclude();
          modelBuilder.Entity<Movie>().Navigation(movie => movie.Director).AutoInclude();
          modelBuilder.Entity<User>().Navigation(user => user.Role).AutoInclude();
          modelBuilder.Entity<User>().Navigation(user => user.RefreshTokens).AutoInclude();
        }

  }

}