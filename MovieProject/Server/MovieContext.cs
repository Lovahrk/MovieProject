using Microsoft.EntityFrameworkCore;
using MovieProject.Shared;

namespace MovieProject.Server
{
    public class MovieContext : DbContext
    {
        public MovieContext(DbContextOptions<MovieContext> options) : base(options) { }
        public DbSet<Movie> Movie { get; set; }
        public DbSet<Director> Director { get; set; }
        public DbSet<Genre> Genre { get; set; }
    }
}
