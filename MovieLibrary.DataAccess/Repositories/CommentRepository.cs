using Microsoft.EntityFrameworkCore;
using MovieLibrary.DataAccess.EF;
using MovieLibrary.DataAccess.Interfaces;
using MovieLibrary.DataAccess.Models;

namespace MovieLibrary.DataAccess.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly MovieLibraryContext _mlContext;

        public CommentRepository(MovieLibraryContext movieLibraryContext)
        {
            _mlContext = movieLibraryContext;
        }

        public async Task<int> CreateAsync(Comment entity)
        {
            await _mlContext.Comments.AddAsync(entity);
            await _mlContext.SaveChangesAsync();
            var entityId = entity.Id;
            return entityId;
        }

        public async Task DeleteAsync(int id)
        {
            var comment = await _mlContext.Comments.FindAsync(id);
            if (comment != null)
            {
                _mlContext.Comments.Remove(comment);
            }

            await _mlContext.SaveChangesAsync();
        }

        public async Task<IList<Comment>?> GetAllAsync()
        {
            var comments = _mlContext.Comments;
            return await comments.ToListAsync();
        }

        public async Task<IList<Comment>> GetAllByMovieId(int id)
        {
            var comments = _mlContext.Comments.Where(comment => comment.MovieId == id);
            return await comments.ToListAsync();
        }

        public async Task<IList<Comment>> GetAllByUserNickname(string nick)
        {
            var comments = _mlContext.Comments.Where(comment => comment.UserNickname == nick);
            return await comments.ToListAsync();
        }

        public async Task<Comment?> GetByIdAsync(int id)
        {
            var comment = await _mlContext.Comments.FindAsync(id);
            return comment;
        }

        public async Task UpdateAsync(Comment entity)
        {
            var local = _mlContext.Comments.Local.FirstOrDefault(entry => entry.Id.Equals(entity.Id));

            if (local != null)
            {
                _mlContext.Entry(local).State = EntityState.Detached;
            }

            _mlContext.Entry(entity).State = EntityState.Modified;
            await _mlContext.SaveChangesAsync();
        }
    }
}
