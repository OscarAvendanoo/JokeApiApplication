using JokeWebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;

namespace JokeWebApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
          
           
            return View();
        }
        public async Task<IActionResult> DisplayJoke(bool NSFW = false)
        {
            using HttpClient client = new HttpClient();
      
            Random random = new Random();

            int randomNumber = random.Next(0, 5);

            var ListOfGenres = new List<string>();

            ListOfGenres.Add("https://v2.jokeapi.dev/joke/Programming");
            ListOfGenres.Add("https://v2.jokeapi.dev/joke/Misc");
            ListOfGenres.Add("https://v2.jokeapi.dev/joke/Pun");
            ListOfGenres.Add("https://v2.jokeapi.dev/joke/Spooky");
            ListOfGenres.Add("https://v2.jokeapi.dev/joke/Christmas");

           
            HttpResponseMessage response = await client.GetAsync(ListOfGenres[randomNumber]);
            response.EnsureSuccessStatusCode(); // Throws if not 2xx

            string json = await response.Content.ReadAsStringAsync();

            // Deserialize JSON response
            var jokeData = JsonSerializer.Deserialize<JokeResponse>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
			if (jokeData.Safe)
			{
				return View(jokeData);
			}
			else
			{
				return RedirectToAction("DisplayJoke");
			}

			return View(jokeData);
            
            
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
