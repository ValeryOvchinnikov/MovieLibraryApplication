using Microsoft.EntityFrameworkCore;
using MovieLibrary.DataAccess.EF;
using MovieLibrary.DataAccess.Interfaces;
using MovieLibrary.DataAccess.Models;

namespace MovieLibrary.DataAccess.Repositories
{
    public class RatingRepository : IRatingRepository
    {
        private readonly MovieLibraryContext _mlContext;

        public RatingRepository(MovieLibraryContext mlContext)
        {
            _mlContext = mlContext;
        }

        public async Task<int> CreateAsync(Rating entity)
        {
            await _mlContext.Ratings.AddAsync(entity);
            await _mlContext.SaveChangesAsync();
            var entityId = entity.Id;
            return entityId;
        }

        public async Task DeleteAsync(int id)
        {
            var rating = await _mlContext.Ratings.FindAsync(id);
            if (rating != null)
            {
                _mlContext.Ratings.Remove(rating);
            }

            await _mlContext.SaveChangesAsync();
        }

        public async Task<IList<Rating>?> GetAllAsync()
        {
            var ratings = _mlContext.Ratings;
            return await ratings.ToListAsync();
        }

        public async Task<IList<Rating>> GetAllByMovieId(int id)
        {
            var ratings = _mlContext.Ratings.Where(rating => rating.MovieId == id);
            return await ratings.ToListAsync();
        }

        public async Task<IList<Rating>> GetAllByUserNickname(string nick)
        {
            var ratings = _mlContext.Ratings.Where(comment => comment.UserNickname == nick);
            return await ratings.ToListAsync();
        }

        public async Task<Rating?> GetByIdAsync(int id)
        {
            var rating = await _mlContext.Ratings.FindAsync(id);
            return rating;
        }

        public async Task UpdateAsync(Rating entity)
        {
            var local = _mlContext.Ratings.Local.FirstOrDefault(entry => entry.Id.Equals(entity.Id));

            if (local != null)
            {
                _mlContext.Entry(local).State = EntityState.Detached;
            }

            _mlContext.Entry(entity).State = EntityState.Modified;
            await _mlContext.SaveChangesAsync();
        }
    }
}
