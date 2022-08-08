using MovieLibrary.DataAccess.Models;

namespace MovieLibrary.DataAccess.Interfaces
{
    public interface ICommentRepository : IRepository<Comment>
    {
        Task<IList<Comment>> GetAllByMovieId(int id);

        Task<IList<Comment>> GetAllByUserNickname(string nick);
    }
}
