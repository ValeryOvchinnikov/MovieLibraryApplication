using MovieLibrary.BusinessLogic.Models;

namespace MovieLibrary.BusinessLogic.Interfaces
{
    public interface IRatingService
    {
        Task<double> GetAverageRating(int id);
        Task<IEnumerable<RatingDTO>> GetRatingsByMovieId(int id);
        Task<IEnumerable<RatingDTO>> GetRatingsByUserNickname(string nick);
        Task<int> CreateRating(RatingDTO rating);
        Task UpdateRating(RatingDTO rating);
        Task DeleteRating(int id);
    }
}
