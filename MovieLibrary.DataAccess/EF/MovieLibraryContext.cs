using Microsoft.EntityFrameworkCore;
using MovieLibrary.DataAccess.Models;

namespace MovieLibrary.DataAccess.EF
{
    public class MovieLibraryContext : DbContext
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public MovieLibraryContext(DbContextOptions<MovieLibraryContext> options)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
          : base(options)
        {
        }

        public DbSet<Movie> Movies { get; set; }

        public DbSet<Director> Directors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Movie>().HasOne(d => d.Director).WithMany(m => m.Movies).HasForeignKey(m => m.DirectorId).HasPrincipalKey(m => m.Id).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
