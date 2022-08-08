using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieLibrary.BusinessLogic.Infrastructure;
using MovieLibrary.BusinessLogic.Interfaces;
using MovieLibrary.BusinessLogic.Models;

namespace MovieLibrary.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController : Controller
    {
        private readonly IMovieService _movieService;
        private readonly IRatingService _ratingService;
        private readonly ICommentService _commentService;

        public MoviesController(IMovieService movieService, ICommentService commentService, IRatingService ratingService)
        {
            _movieService = movieService;
            _commentService = commentService;
            _ratingService = ratingService;
        }

        [HttpGet("GetMovies")]
        public async Task<ActionResult<List<MovieDTO>>> GetMovies()
        {
            var movieList = await _movieService.GetAllMovies();
            return Ok(movieList);
        }

        [HttpGet("{id}", Name = "GetMovieById")]
        public async Task<ActionResult<MovieDTO>> GetMovieById([FromRoute] int id)
        {
            var movie = await _movieService.GetMovie(id);
            return Ok(movie);
        }
        [HttpPost(Name = "CreateMovie")]
        public async Task<ActionResult> CreateMovie([FromBody] MovieDTO movie)
        {
            if (movie == null)
            {
                return BadRequest();
            }

            var newMovieId = await _movieService.CreateMovie(movie);
            return Created(new Uri($"api/Movies/{newMovieId}", UriKind.Relative), new { message = "New movie added" });
        }

        [Authorize]
        [HttpDelete("{id}", Name = "DeleteMovie")]
        public async Task<ActionResult> DeleteMovie([FromRoute] int id)
        {
            await _movieService.DeleteMovie(id);
            return Ok();
        }

        [Authorize]
        [HttpGet(Name = "GetMoviesByFilter")]
        public async Task<ActionResult<List<MovieDTO>>> GetMoviesByFilter([FromQuery] Filter filter)
        {
            var movieList = await _movieService.GetFilteredMovies(filter);
            return Ok(movieList);
        }

        [Authorize]
        [HttpPut(Name = "UpdateMovie")]
        public async Task<ActionResult> UpdateMovie([FromBody] MovieDTO movie)
        {
            if (movie != null)
            {
                await _movieService.UpdateMovie(movie);
            }

            return Ok(movie);
        }


        [HttpPost("AddComment")]
        public async Task<ActionResult> CreateComment([FromBody] CommentDTO comment)
        {
            if (comment == null)
            {
                return BadRequest();
            }

            var id = await _commentService.CreateComment(comment);
            return Created(new Uri($"api/Movies/{id}", UriKind.Relative), new { message = "New comment added" });
        }

        [HttpGet("{movieId}/Comments", Name = "GetAllCommentsByMovie")]
        public async Task<ActionResult<List<CommentDTO>>> GetCommentsByMovie([FromRoute] int movieId)
        {
            var comments = await _commentService.GetCommentsByMovieId(movieId);
            return Ok(comments);
        }

        [HttpDelete("DeleteComment/{commentId}", Name = "DeleteComment")]
        public async Task<ActionResult> DeleteComment([FromRoute] int commentId)
        {
            await _commentService.DeleteComment(commentId);
            return Ok();
        }


        [HttpPut("UpdateComment")]
        public async Task<ActionResult> UpdateComment([FromBody] CommentDTO comment)
        {
            if (comment != null)
            {
                await _commentService.UpdateComment(comment);
            }

            return Ok(comment);
        }

        [HttpPost("AddRating")]
        public async Task<ActionResult> AddRating([FromBody] RatingDTO rating)
        {
            if (rating == null)
            {
                return BadRequest();
            }
            await _ratingService.CreateRating(rating);
            return Created(new Uri($"api/Movies/{rating.MovieId}", UriKind.Relative), new { message = "New rating added" });
        }

        [HttpDelete("DeleteRating")]
        public async Task<ActionResult> DeleteRating(int ratingId)
        {
            await _ratingService.DeleteRating(ratingId);
            return Ok();
        }

        [HttpGet("GetAvarageRating")]
        public async Task<ActionResult> GetAvarageRating(int movieId)
        {
            var avgRating = await _ratingService.GetAverageRating(movieId);
            return Ok(avgRating);
        }

        [HttpGet("{movieId}/GetRatingsByMovie")]
        public async Task<ActionResult<List<RatingDTO>>> GetRatingsByMovie([FromRoute]int movieId)
        {
            var avgRating = await _ratingService.GetRatingsByMovieId(movieId);
            return Ok(avgRating);
        }

    }
}
