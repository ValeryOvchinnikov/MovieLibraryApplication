using LinqKit;
using Microsoft.EntityFrameworkCore;
using MovieLibrary.DataAccess.EF;
using MovieLibrary.DataAccess.Interfaces;
using MovieLibrary.DataAccess.Models;

namespace MovieLibrary.DataAccess.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly MovieLibraryContext _mlContext;

        public MovieRepository(MovieLibraryContext movieLibraryContext)
        {
            _mlContext = movieLibraryContext;
        }

        public async Task<int> CreateAsync(Movie entity)
        {
            await _mlContext.Movies.AddAsync(entity);
            await _mlContext.SaveChangesAsync();
            var entityId = entity.Id;
            return entityId;
        }

        public async Task CreateRangeAsync(List<Movie> movies)
        {
            await _mlContext.Movies.AddRangeAsync(movies);
            await _mlContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var movie = await _mlContext.Movies.FindAsync(id);
            if (movie != null)
            {
                _mlContext.Movies.Remove(movie);
            }

            await _mlContext.SaveChangesAsync();
        }

        public async Task<IList<Movie>?> GetAllAsync()
        {
            var movies = _mlContext.Movies;
            return await movies.ToListAsync();
        }

        public async Task<Movie?> GetByIdAsync(int id)
        {
            var movie = await _mlContext.Movies.FindAsync(id);
            return movie;
        }

        public async Task<List<Movie>> GetWhere(ExpressionStarter<Movie> condition)
        {
            var movies = _mlContext.Movies.Where(condition);
            return await movies.ToListAsync();
        }

        public async Task UpdateAsync(Movie entity)
        {
            var local = _mlContext.Movies.Local.FirstOrDefault(entry => entry.Id.Equals(entity.Id));

            if (local != null)
            {
                _mlContext.Entry(local).State = EntityState.Detached;
            }
            _mlContext.Entry(entity).State = EntityState.Modified;
            await _mlContext.SaveChangesAsync();
        }
    }
}
