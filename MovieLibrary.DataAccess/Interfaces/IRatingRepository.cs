using MovieLibrary.DataAccess.Models;

namespace MovieLibrary.DataAccess.Interfaces
{
    public interface IRatingRepository : IRepository<Rating>
    {
        Task<IList<Rating>> GetAllByMovieId(int id);

        Task<IList<Rating>> GetAllByUserNickname(string nick);
    }
}
