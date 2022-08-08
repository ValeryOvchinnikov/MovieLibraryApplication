using AutoMapper;
using MovieLibrary.BusinessLogic.Infrastructure;
using MovieLibrary.BusinessLogic.Infrastructure.Logger;
using MovieLibrary.BusinessLogic.Interfaces;
using MovieLibrary.BusinessLogic.Models;
using MovieLibrary.DataAccess.Interfaces;
using MovieLibrary.DataAccess.Models;

namespace MovieLibrary.BusinessLogic.Services
{
    public class RatingService : IRatingService
    {
        private ILog _logger;
        private readonly IMovieRepository _movieRepository;
        private readonly IRatingRepository _ratingRepository;
        private readonly IMapper _mapper;

        public RatingService(IMovieRepository movieRepository, IRatingRepository ratingRepository, IMapper mapper)
        {
            _logger = Log.GetInstance;
            _movieRepository = movieRepository;
            _ratingRepository = ratingRepository;
            _mapper = mapper;
        }

        public async Task<double> GetAverageRating(int id)
        {
            var temp = await _ratingRepository.GetAllByMovieId(id) as List<Rating>;
            if (temp == null)
            {
                throw new ValidationException("Ratings for this movie was not found");
            }

            List<int> avgRating = new();
            if (temp.Count > 0)
            {
                foreach (Rating rating in temp)
                {
                    avgRating.Add(rating.Value);
                }
                double avg = avgRating.Average();

                return Math.Round(avg, 1); ;
            }
            else
            {
                return 0;
            }


        }

        public async Task<IEnumerable<RatingDTO>> GetRatingsByMovieId(int id)
        {
            List<Rating>? ratings = await _ratingRepository.GetAllByMovieId(id) as List<Rating>;

            IEnumerable<RatingDTO> mappedRatings = ratings!.Select(rating => _mapper.Map<RatingDTO>(rating));
            return mappedRatings;
        }

        public async Task<IEnumerable<RatingDTO>> GetRatingsByUserNickname(string nick)
        {
            List<Rating>? ratings = await _ratingRepository.GetAllByUserNickname(nick) as List<Rating>;

            IEnumerable<RatingDTO> mappedRatings = ratings!.Select(rating => _mapper.Map<RatingDTO>(rating));
            return mappedRatings;
        }

        public async Task<int> CreateRating(RatingDTO rating)
        {
            if (rating == null)
            {
                throw new ValidationException("Data not found");
            }

            var mappedRating = _mapper.Map<Rating>(rating);

            Movie? movie = await _movieRepository.GetByIdAsync(rating.MovieId);

            if (movie == null)
            {
                throw new ValidationException("Movie was not found");
            }
            else
            {
                var newRatingId = await _ratingRepository.CreateAsync(mappedRating);
                return newRatingId;
            }
        }

        public async Task UpdateRating(RatingDTO rating)
        {
            if (rating == null)
            {
                throw new ValidationException("Data not found");
            }

            var oldRating = await _ratingRepository.GetByIdAsync(rating.Id);

            if (oldRating == null)
            {
                throw new ValidationException("Comment not found");
            }

            var newRating = _mapper.Map<Rating>(rating);
            await _ratingRepository.UpdateAsync(newRating);
        }

        public async Task DeleteRating(int id)
        {
            if (await _ratingRepository.GetByIdAsync(id) == null)
            {
                throw new ValidationException("Rating was not found");
            }

            await _ratingRepository.DeleteAsync(id);
        }
    }
}
