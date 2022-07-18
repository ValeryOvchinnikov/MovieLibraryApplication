using LinqKit;
using MovieLibrary.BusinessLogic.Models;
using MovieLibrary.DataAccess.Models;

namespace MovieLibrary.BusinessLogic.Interfaces
{
    public interface IMovieService
    {
        Task<int> CreateMovie(MovieDTO movie);

        Task UpdateMovie(MovieDTO movie);

        Task DeleteMovie(int id);

        Task<MovieDTO?> GetMovie(int id);

        Task<IEnumerable<MovieDTO>?> GetFilteredMovies(ExpressionStarter<Movie> filter);

        Task<IList<DirectorDTO>?> GetAllDirectors();

        Task<IList<MovieDTO>?> GetAllMovies();
    }
}
