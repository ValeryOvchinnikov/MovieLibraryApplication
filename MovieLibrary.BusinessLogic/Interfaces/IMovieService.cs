using MovieLibrary.BusinessLogic.Infrastructure;
using MovieLibrary.BusinessLogic.Models;

namespace MovieLibrary.BusinessLogic.Interfaces
{
    public interface IMovieService
    {
        Task<int> CreateMovie(MovieDTO movie);

        Task CreateMovieList(List<MovieDTO> movieList);

        Task UpdateMovie(MovieDTO movie);

        Task DeleteMovie(int id);

        Task<MovieDTO?> GetMovie(int id);

        Task<IEnumerable<MovieDTO>?> GetFilteredMovies(Filter filter);

        Task<IEnumerable<DirectorDTO>?> GetAllDirectors();

        Task<IEnumerable<MovieDTO>?> GetAllMovies();
    }
}
