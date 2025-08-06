using Aris3._0.Application.DTOs;
using Aris3._0.Application.Interface.Service;
using Aris3._0.Application.Service;
using Aris3._0.Domain.Entities;
using Aris3._0.Infrastructure.Data.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace Aris3._0.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilmController : ControllerBase
    {
        private readonly HttpClient client;
        private readonly ArisDbContext _context;
        private readonly IFilmService filmService;

        public FilmController(HttpClient client, ArisDbContext dbContext, IFilmService filmService)
        {
            this.client = client;
            _context = dbContext;
            this.filmService = filmService;
        }

        public ArisDbContext DbContext { get; }

        [HttpGet]
        [Route("slug/{slug}")]
        public async Task<IActionResult> GetFilmsBySlug(string slug)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://phimapi.com/phim/{slug}");
            request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, "Failed to fetch data");

            var content = await response.Content.ReadAsStringAsync();
            var json = JObject.Parse(content);

            return Ok(json);
        }
        [HttpPost("Get-Film-To-Db-By-Slug")]
        public async Task<IActionResult> GetFilmToDbBySlug(string slug)
        {
            var result = await filmService.ImportFilmFromSlugAsync(slug);
            if (result == null)
                return BadRequest("Error");

            return Ok("Added successfully");
        }
        [HttpPost("{pages}")]
        public async Task<IActionResult> GetAllNewUpdatedFilmToDb(int pages)
        {
            int curr_item = _context.Films.Count();
            int after_item = 0;
            after_item = await filmService.ImportAllNewUpdatedFilmsAsync(pages);
            if (after_item != 0)
            {
                after_item = _context.Films.Count();
                return Ok(new
                {
                    status = true,
                    msg = "Added" + " " + (after_item - curr_item) + " " + "New Films !"
                });
            }
            else
            {
                return BadRequest("Nothing added");
            }
        }
        [HttpGet("From-Db")]
        public async Task<IActionResult> GetAllFromDb()
        {
            var Films = _context.Films
                                .Include(f => f.Actors)
                                .Include(f => f.Categories)
                                .Include(f => f.Countries)
                                .ToList();
            return Ok(Films);
        }
    }
}


