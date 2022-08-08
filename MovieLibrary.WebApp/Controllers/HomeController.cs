using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieLibrary.WebApp.Models;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http.Headers;

namespace MovieLibrary.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        HttpClient client = new HttpClient();
        string apiUrl = "https://localhost:6001/api/Movies";

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> MovieList()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await client.GetAsync(apiUrl + "/GetMovies");
            var movies = new List<MovieDTO>();
            if (response.IsSuccessStatusCode)
            {
                movies = JsonConvert.DeserializeObject<List<MovieDTO>>(await response.Content.ReadAsStringAsync());
            }
            return View(movies);
        }

        [Authorize]
        public async Task<IActionResult> Login()
        {
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Manage()
        {
            return Redirect("https://localhost:5001/Account/Manage");
        }
        public IActionResult Logout()
        {
            return SignOut("Cookies", "oidc");
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}