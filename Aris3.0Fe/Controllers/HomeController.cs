using Aris3._0.Infrastructure.Data.Context;
using Aris3._0Fe.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http;

namespace Aris3._0Fe.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ArisDbContext dbContext;
        private readonly HttpClient _httpClient;
        public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory,ArisDbContext dbContext)
        {
            _httpClient = httpClientFactory.CreateClient();
            _logger = logger;
            this.dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var listFilm = dbContext.Films.ToList();
            ViewBag.FilmList = listFilm;
            return View(listFilm);
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
        [HttpPost]
        public async Task<IActionResult> GetAllFilmToDb()
        {
            var response = await _httpClient.PostAsync("https://localhost:7248/api/film", null);

            var resultJson = await response.Content.ReadAsStringAsync();

            return RedirectToAction("Index");
        }
    }
}
