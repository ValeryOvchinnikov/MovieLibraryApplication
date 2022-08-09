using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieLibrary.WebApp.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;

namespace MovieLibrary.WebApp.Controllers
{
    public class MovieController : Controller
    {
        private readonly ILogger<MovieController> _logger;
        readonly HttpClient client = new HttpClient();
        readonly string apiUrl = "https://localhost:6001/api/Movies";

        public MovieController(ILogger<MovieController> logger)
        {
            _logger = logger;
        }

        // GET: MovieController
        public ActionResult Index()
        {
            return View();
        }

        // GET: MovieController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var movieResponse = await client.GetAsync($"{apiUrl}/{id}");
            var movie = new MovieDTO();
            if (movieResponse.IsSuccessStatusCode)
            {
                movie = JsonConvert.DeserializeObject<MovieDTO>(await movieResponse.Content.ReadAsStringAsync());
            }

            var response = await client.GetAsync($"{apiUrl}/{id}/Comments");
            var comments = new List<CommentDTO>();
            if (response.IsSuccessStatusCode)
            {
                comments = JsonConvert.DeserializeObject<List<CommentDTO>>(await response.Content.ReadAsStringAsync());
            }
            ViewBag.Comments = comments;
            return View(movie);
        }

        // GET: MovieController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MovieController/Create
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(MovieDTO movie)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await client.PostAsJsonAsync($"{apiUrl}", movie);
            var temp = response.Headers.Location.ToString();
            var regex = new Regex(@"\d*$");
            var match = regex.Match(temp);
            var parsedResponse = JsonConvert.DeserializeObject<MovieDTO>(await response.Content.ReadAsStringAsync());
            if (response.IsSuccessStatusCode)
            {
                ViewBag.Result = "Success";
                return RedirectToAction("Details", new { id = match.Value });
            }
            else
            {
                return NotFound();
            }
        }

        // GET: MovieController/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(int id)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var movieResponse = await client.GetAsync($"{apiUrl}/{id}");
            var movie = new MovieDTO();
            if (movieResponse.IsSuccessStatusCode)
            {
                movie = JsonConvert.DeserializeObject<MovieDTO>(await movieResponse.Content.ReadAsStringAsync());
            }
            return View(movie);
        }

        // POST: MovieController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(MovieDTO movie)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await client.PutAsJsonAsync($"{apiUrl}", movie);
            if (response.IsSuccessStatusCode)
            {
                ViewBag.Result = "Success";
                return RedirectToAction("Details", new { id = movie.Id });
            }
            else
            {
                return NotFound();
            }
        }

        // GET: MovieController/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await client.DeleteAsync($"{apiUrl}/?id={id}");

            if (response.IsSuccessStatusCode)
            {
                ViewBag.Result = "Success";
                return RedirectToAction("MovieList", "Home");
            }
            else
            {
                return NotFound();
            }
        }

        // POST: MovieController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<ActionResult> PartialCreateComment([FromForm] CommentDTO comment)
        {
            comment.Id = 0;
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await client.PostAsJsonAsync($"{apiUrl}/AddComment", comment);
            if (response.IsSuccessStatusCode)
            {
                ViewBag.Result = "Success";
                return RedirectToAction("Details", new { id = comment.MovieId });
            }
            else
            {
                return NotFound();
            }
        }
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteComment(int id, int movieId)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await client.DeleteAsync($"{apiUrl}/DeleteComment/{id}");

            if (response.IsSuccessStatusCode)
            {
                ViewBag.Result = "Success";
                return RedirectToAction("Details", new { id = movieId });
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        public async Task<ActionResult> PartialCommentList(int id)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await client.GetAsync($"{apiUrl}/{id}/Comments");
            var comments = new List<CommentDTO>();
            if (response.IsSuccessStatusCode)
            {
                comments = JsonConvert.DeserializeObject<List<CommentDTO>>(await response.Content.ReadAsStringAsync());
            }
            return PartialView(comments);
        }


        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<ActionResult> AddRating([FromForm]RatingDTO rating)
        {
            rating.Id = 0;
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await client.PostAsJsonAsync($"{apiUrl}/AddRating", rating);
            if (response.IsSuccessStatusCode)
            {
                ViewBag.Result = "Success";
                return RedirectToAction("Details", new { id = rating.MovieId });
            }
            else
            {
                return NotFound();
            }
        }

        public async Task<ActionResult> RatingList(int movieId)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await client.GetAsync($"{apiUrl}/{movieId}/GetRatingsByMovie/");
            var ratings = new List<RatingDTO>();
            if (response.IsSuccessStatusCode)
            {
                ratings = JsonConvert.DeserializeObject<List<RatingDTO>>(await response.Content.ReadAsStringAsync());
            }
            return View(ratings);
        }
    }
}
