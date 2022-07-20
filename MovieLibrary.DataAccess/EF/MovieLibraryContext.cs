using Microsoft.EntityFrameworkCore;
using MovieLibrary.DataAccess.Models;

namespace MovieLibrary.DataAccess.EF
{
    public class MovieLibraryContext : DbContext
    {
        public MovieLibraryContext(DbContextOptions<MovieLibraryContext> options)
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
