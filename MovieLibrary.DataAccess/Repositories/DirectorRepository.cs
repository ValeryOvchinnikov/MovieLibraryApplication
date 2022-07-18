using Microsoft.EntityFrameworkCore;
using MovieLibrary.DataAccess.EF;
using MovieLibrary.DataAccess.Interfaces;
using MovieLibrary.DataAccess.Models;

namespace MovieLibrary.DataAccess.Repositories
{
    public class DirectorRepository : IDirectorRepository
    {
        private readonly MovieLibraryContext _mlContext;

        public DirectorRepository(MovieLibraryContext movieLibraryContext)
        {
            _mlContext = movieLibraryContext;
        }

        public async Task<int> CreateAsync(Director entity)
        {
            await _mlContext.Directors.AddAsync(entity);
            await _mlContext.SaveChangesAsync();
            var entityId = entity.Id;
            return entityId;
        }

        public async Task DeleteAsync(int id)
        {
            Director? director = await _mlContext.Directors.FindAsync(id);
            if (director != null)
            {
                _mlContext.Directors.Remove(director);
            }

            await _mlContext.SaveChangesAsync();
        }

        public async Task<IList<Director>?> GetAllAsync()
        {
            var directors = _mlContext.Directors;
            return await directors.ToListAsync();
        }

        public async Task<Director?> GetByIdAsync(int id)
        {
            Director? director = await _mlContext.Directors.FindAsync(id);
            return director;
        }

        public async Task UpdateAsync(Director entity)
        {
            _mlContext.Directors.Update(entity);
            await _mlContext.SaveChangesAsync();
        }
    }
}
