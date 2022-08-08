using AutoMapper;
using LinqKit;
using MovieLibrary.BusinessLogic.Infrastructure;
using MovieLibrary.BusinessLogic.Infrastructure.Logger;
using MovieLibrary.BusinessLogic.Interfaces;
using MovieLibrary.BusinessLogic.Models;
using MovieLibrary.DataAccess.Interfaces;
using MovieLibrary.DataAccess.Models;

namespace MovieLibrary.BusinessLogic.Services
{
    public class MovieService : IMovieService
    {
        private ILog _logger;
        private readonly IRatingService _ratingService;
        private readonly IMovieRepository _movieRepository;
        private readonly IDirectorRepository _directorRepository;
        private readonly IRatingRepository _ratingRepository;
        private readonly IMapper _mapper;

        public MovieService(IRatingService ratingService, IMovieRepository movieRepository, IDirectorRepository directorRepository, IRatingRepository ratingRepository, IMapper mapper)
        {
            _ratingService = ratingService;
            _movieRepository = movieRepository;
            _directorRepository = directorRepository;
            _ratingRepository = ratingRepository;
            _mapper = mapper;
            _logger = Log.GetInstance;
        }

        public async Task<int> CreateMovie(MovieDTO movie)
        {
            if (movie == null)
            {
                _logger.LogException("Data not found");
                throw new ValidationException("Data not found");
            }

            var mappedMovie = _mapper.Map<Movie>(movie);

            List<Director>? directors = await _directorRepository.GetAllAsync() as List<Director>;
            IEnumerable<Director> selectedDirectors = from d in directors
                                                      where d.FirstName == mappedMovie!.Director!.FirstName
                                                      where d.LastName == mappedMovie!.Director!.LastName
                                                      select d;

            Director? selectedDirector = selectedDirectors.FirstOrDefault();

            if (selectedDirector == null)
            {
                var movieId = await _movieRepository.CreateAsync(mappedMovie);
                return movieId;
            }
            else
            {
                IEnumerable<Movie> directorsMovies = from m in selectedDirector.Movies
                                                     where m.Name == mappedMovie.Name
                                                     where m.Year == mappedMovie.Year
                                                     select m;

                Movie? mv = directorsMovies.FirstOrDefault();
                if (mv != null)
                {
                    _logger.LogException("Such movie already exist");
                    throw new ValidationException("Such movie already exist");
                }
                else
                {
                    mappedMovie.DirectorId = selectedDirector.Id;
                    mappedMovie.Director = selectedDirector;
                    var movieId = await _movieRepository.CreateAsync(mappedMovie);
                    return movieId;
                }
            }
        }

        public async Task DeleteMovie(int id)
        {
            if (await _movieRepository.GetByIdAsync(id) == null)
            {
                OnException("Movie was not found");
            }

            await _movieRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<MovieDTO>?> GetFilteredMovies(Filter filter)
        {
            ExpressionStarter<Movie> filterExpressions;
            filterExpressions = PredicateBuilder.New<Movie>().And(m => true);
            if (!string.IsNullOrEmpty(filter.FilterMovieName))
            {
                filterExpressions.And(m => m.Name.Contains(filter.FilterMovieName));
            }
            if (!string.IsNullOrEmpty(filter.FilterMovieYear))
            {
                filterExpressions.And(m => m.Year.ToString().Contains(filter.FilterMovieYear));
            }
            if (!string.IsNullOrEmpty(filter.FilterDirectorFirstName))
            {
                filterExpressions.And(m => m.Director.FirstName.Contains(filter.FilterDirectorFirstName));
            }
            if (!string.IsNullOrEmpty(filter.FilterDirectorLastName))
            {
                filterExpressions.And(m => m.Director.LastName.Contains(filter.FilterDirectorLastName));
            }
            if (!string.IsNullOrEmpty(filter.FilterMovieRating))
            {
                filterExpressions.And(m => m.Rating.ToString() == filter.FilterMovieRating.ToString());
            }
            List<Movie>? movies = await _movieRepository.GetWhere(filterExpressions);
            IEnumerable<MovieDTO>? mappedMovies = movies!.Select(movie => _mapper.Map<MovieDTO>(movie));
            return mappedMovies.ToList();
        }

        public async Task<IEnumerable<MovieDTO>?> GetAllMovies()
        {
            List<Movie>? movies = await _movieRepository.GetAllAsync() as List<Movie>;
            foreach (Movie movie in movies)
            {
                movie.Rating = await _ratingService.GetAverageRating(movie.Id);
                Math.Round(movie.Rating, 1);
            }
            IEnumerable<MovieDTO> mappedMovies = movies!.Select(movie => _mapper.Map<MovieDTO>(movie));
            return mappedMovies;
        }

        public async Task<MovieDTO?> GetMovie(int id)
        {
            Movie? movie = await _movieRepository.GetByIdAsync(id);
            if (movie == null)
            {
                OnException("Movie was not found");
            }

            movie.Rating = await _ratingService.GetAverageRating(movie.Id);
            Math.Round(movie.Rating, 1);

            var movieDTO = _mapper.Map<MovieDTO>(movie);

            return movieDTO;
        }

        public async Task UpdateMovie(MovieDTO movie)
        {
            if (movie == null)
            {
                OnException("Data not found");
            }

            var oldMovie = await _movieRepository.GetByIdAsync(movie.Id);

            if (oldMovie == null)
            {
                OnException("Movie not found");
            }

            var newMovie = _mapper.Map<Movie>(movie);
            await _movieRepository.UpdateAsync(newMovie);
        }

        public async Task<IEnumerable<DirectorDTO>?> GetAllDirectors()
        {
            List<Director>? directors = await _directorRepository.GetAllAsync() as List<Director>;
            var mappedDirectors = directors?.Select(d => _mapper.Map<DirectorDTO>(d));
            return mappedDirectors?.ToList();
        }

        public async Task CreateMovieList(List<MovieDTO> movieList)
        {
            if (movieList == null)
            {
                OnException("List of movie is empty");
            }
            List<Movie>? mappedMovies = movieList.Select(m => _mapper.Map<Movie>(m)) as List<Movie>;
            await _movieRepository.CreateRangeAsync(mappedMovies);
        }

        protected void OnException(string message)
        {
            _logger.LogException(message);
            throw new ValidationException(message);
        }
    }
}
