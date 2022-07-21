using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieLibrary.BusinessLogic.Infrastructure;
using MovieLibrary.BusinessLogic.Interfaces;
using MovieLibrary.BusinessLogic.Models;

namespace MovieLibrary.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class MoviesController : Controller
    {
        private readonly IMovieService _movieService;

        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        [HttpGet("GetMovies")]
        public async Task<ActionResult<List<MovieDTO>>> GetMovies()
        {
            var movieList = await _movieService.GetAllMovies();
            return Ok(movieList);
        }

        [HttpGet("{id}", Name = "GetMovieById")]
        public async Task<ActionResult<MovieDTO>> GetMovie(int id)
        {
            var movie = await _movieService.GetMovie(id);
            return Ok(movie);
        }

        [HttpPost("CreateMovie")]
        public async Task<ActionResult> CreateMovie(MovieDTO movie)
        {
            if (movie == null)
            {
                return BadRequest();
            }

            var newMovieId = await _movieService.CreateMovie(movie);
            return Created(new Uri($"api/Movies/{newMovieId}", UriKind.Relative), new { message = "New movie added" });
        }

        [HttpDelete("{id}", Name = "DeleteMovie")]
        public async Task<ActionResult> Delete(int id)
        {
            await _movieService.DeleteMovie(id);
            return Ok();
        }

        [HttpGet("GetDirectors")]
        public async Task<ActionResult<List<DirectorDTO>>> Get()
        {
            var directorsList = await _movieService.GetAllDirectors();
            return Ok(directorsList);
        }

        [HttpGet("GetMoviesByFilter")]
        public async Task<ActionResult<List<MovieDTO>>> GetMoviesByFilter([FromQuery] Filter filter)
        {
            var movieList = await _movieService.GetFilteredMovies(filter);
            return Ok(movieList);
        }

        [HttpPut("{id}", Name = "UpdateMovie")]
        public async Task<ActionResult> Put(MovieDTO movie)
        {
            if (movie != null)
            {
                await _movieService.UpdateMovie(movie);
            }

            return Ok(movie);
        }
    }
}
