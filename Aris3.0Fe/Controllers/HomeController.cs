using Aris3._0Fe.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http;

namespace Aris3._0Fe.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient _httpClient;
        public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [HttpGet]
        public async Task<IActionResult> GetAllFilmToDb()
        {
            var response = await _httpClient.PostAsync("https://localhost:7248/api/film", null);

            var resultJson = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ImportResponse>(resultJson);

            TempData["Message"] = result?.msg ?? $"Call failed with status {response.StatusCode}";
            return RedirectToAction("Index");
        }


        public class ImportResponse
        {
            public bool status { get; set; }
            public string msg { get; set; }
        }
    }
    }
