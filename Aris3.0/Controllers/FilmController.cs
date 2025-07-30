using Aris3._0.Application.DTOs;
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
        public FilmController(HttpClient client, ArisDbContext dbContext)
        {
            this.client = client;
            _context = dbContext;
        }

        public ArisDbContext DbContext { get; }

        [HttpGet]
        [Route("slug/{slug}")]
        public async Task<IActionResult> GetFilmsBySlug(string slug)
        {
            HttpResponseMessage response;
            response = await client.GetAsync($"https://phimapi.com/phim/{slug}");
            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, "Failed to fetch data");
            var content = await response.Content.ReadAsStringAsync();
            var jObj = JObject.Parse(content);
            var movie = jObj["movie"];
            if (movie == null)
                return BadRequest("No Film Found");
            var film = movie.ToObject<Film>();
            var tmbd = movie["tmdb"] as JObject;

            if (tmbd == null)
            {
                return Ok(new
                {
                    Movie = film,
                    Tmbd = "No episode data"
                });
            }
            else
            {
                var tmbd3 = tmbd.ToObject<Tmbd>();
                return Ok(new
                {
                    Movie = film,
                    Tmbd = tmbd3
                });
            }
        }
        [HttpPost]
        [Route("slug/{slug}")]
        public async Task<IActionResult> GetFilmToDb(string slug)
        {
            HttpResponseMessage response = await client.GetAsync($"https://phimapi.com/phim/{slug}");
            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, "Failed to fetch data");

            var content = await response.Content.ReadAsStringAsync();
            var jObj = JObject.Parse(content);
            var movie = jObj["movie"];
            if (movie == null)
                return BadRequest("No Film Found");

            var tmbdToken = movie["tmdb"];
            var tmbdId = tmbdToken?["id"]?.ToString();
            var episodes = jObj["episodes"];

            if (string.IsNullOrEmpty(tmbdId))
                return BadRequest("Cannot add film with null TMDB id");

            // Find film by tmbd.id
            var existingFilm = await _context.Films
                .Include(f => f.Actors)
                .Include(f => f.Categories)
                .Include(f => f.Countries)
                .Include(f => f.Episodes)
                .FirstOrDefaultAsync(f => f.Tmdb != null && f.Tmdb.Id == tmbdId);

            var filmData = movie.ToObject<Film>();

            // Cập nhật navigation properties
            var actorNames = movie["actor"]?.ToObject<List<string>>() ?? new List<string>();
            filmData.Actors = new List<Actor>();
            foreach (var name in actorNames)
            {
                var actor = await _context.Actors.FirstOrDefaultAsync(a => a.Name == name)
                            ?? new Actor { Name = name };
                filmData.Actors.Add(actor);
            }

            var categoryObjs = movie["category"]?.ToObject<List<JObject>>() ?? new List<JObject>();
            filmData.Categories = new List<Category>();
            foreach (var cat in categoryObjs)
            {
                var slug1 = cat["slug"]?.ToString();
                var existingCategory = await _context.categories.FirstOrDefaultAsync(c => c.Slug == slug1)
                                       ?? new Category
                                       {
                                           Id = cat["id"]?.ToString(),
                                           Name = cat["name"]?.ToString(),
                                           Slug = slug1
                                       };
                filmData.Categories.Add(existingCategory);
            }

            var countryObjs = movie["country"]?.ToObject<List<JObject>>() ?? new List<JObject>();
            filmData.Countries = new List<Country>();
            foreach (var c in countryObjs)
            {
                var slug2 = c["slug"]?.ToString();
                var existingCountry = await _context.Countries.FirstOrDefaultAsync(co => co.Slug == slug2)
                                       ?? new Country
                                       {
                                           Id = Guid.TryParse(c["id"]?.ToString(), out var guid) ? guid : Guid.NewGuid(),
                                           Name = c["name"]?.ToString(),
                                           Slug = slug2
                                       };
                filmData.Countries.Add(existingCountry);
            }

            filmData.Episodes = new List<Episode>();
            foreach (var server in episodes)
            {
                var serverName = server["server_name"]?.ToString();
                var serverData = server["server_data"]?.ToObject<List<JObject>>();
                if (serverData != null)
                {
                    foreach (var ep in serverData)
                    {
                        filmData.Episodes.Add(new Episode
                        {
                            ServerName = serverName,
                            Name = ep["name"]?.ToString(),
                            Slug = ep["slug"]?.ToString(),
                            Filename = ep["filename"]?.ToString(),
                            LinkEmbed = ep["link_embed"]?.ToString(),
                            LinkM3U8 = ep["link_m3u8"]?.ToString()
                        });
                    }
                }
            }

            if (existingFilm != null)
            {
                //  Update fields except Tmdb
                existingFilm.Name = filmData.Name;
                existingFilm.Slug = filmData.Slug;
                existingFilm.OriginName = filmData.OriginName;
                existingFilm.Content = filmData.Content;
                existingFilm.Type = filmData.Type;
                existingFilm.Status = filmData.Status;
                existingFilm.PosterUrl = filmData.PosterUrl;
                existingFilm.ThumbUrl = filmData.ThumbUrl;
                existingFilm.IsCopyright = filmData.IsCopyright;
                existingFilm.SubDocquyen = filmData.SubDocquyen;
                existingFilm.ChieuRap = filmData.ChieuRap;
                existingFilm.TrailerUrl = filmData.TrailerUrl;
                existingFilm.Time = filmData.Time;
                existingFilm.EpisodeCurrent = filmData.EpisodeCurrent;
                existingFilm.EpisodeTotal = filmData.EpisodeTotal;
                existingFilm.Quality = filmData.Quality;
                existingFilm.Lang = filmData.Lang;
                existingFilm.Notify = filmData.Notify;
                existingFilm.Showtimes = filmData.Showtimes;
                existingFilm.Year = filmData.Year;
                existingFilm.View = filmData.View;
                existingFilm.Director = filmData.Director;
                existingFilm.Created = filmData.Created;
                existingFilm.Modified = filmData.Modified;

                // Update navigation properties
                existingFilm.Actors = filmData.Actors;
                existingFilm.Categories = filmData.Categories;
                existingFilm.Countries = filmData.Countries;
                existingFilm.Episodes = filmData.Episodes;

                _context.Films.Update(existingFilm);
            }
            else
            {
                _context.Films.Add(filmData);
            }

            await _context.SaveChangesAsync();
            return Ok(filmData);
        }
        [HttpPost]
        public async Task<IActionResult> GetAllNewUpdatedFilmToDb()
        {
            int curr_item = _context.Films.Count();
            int after_item = curr_item;
            List<string> slugs = new List<string>();

            for (int i = 1; i <= 3; i++)
            {
                var response = await client.GetAsync($"https://phimapi.com/danh-sach/phim-moi-cap-nhat-v2?page={i}");

                if (!response.IsSuccessStatusCode)
                    return StatusCode((int)response.StatusCode, $"Failed to fetch data from page {i}");

                var content = await response.Content.ReadAsStringAsync();
                var jObj = JObject.Parse(content);
                var itemsArray = jObj["items"] as JArray;

                if (itemsArray != null)
                {
                    foreach (var item in itemsArray)
                    {
                        var slug = item["slug"]?.ToString();
                        if (!string.IsNullOrWhiteSpace(slug))
                        {
                            slugs.Add(slug);
                        }
                    }
                }
            }

            foreach (var item in slugs)
            {
                HttpResponseMessage response = await client.GetAsync($"https://phimapi.com/phim/{item}");
                if (!response.IsSuccessStatusCode)
                    continue;

                var content = await response.Content.ReadAsStringAsync();
                var jObj = JObject.Parse(content);
                var movie = jObj["movie"];
                if (movie == null)
                    continue;

                var tmbdToken = movie["tmdb"];
                var tmbdId = tmbdToken?["id"]?.ToString();
                var episodes = jObj["episodes"];

                if (string.IsNullOrEmpty(tmbdId))
                    continue;

                var existingFilm = await _context.Films
                    .Include(f => f.Actors)
                    .Include(f => f.Categories)
                    .Include(f => f.Countries)
                    .Include(f => f.Episodes)
                    .FirstOrDefaultAsync(f => f.Tmdb != null && f.Tmdb.Id == tmbdId);

                var filmData = movie.ToObject<Film>();
                // Process Actors
                var actorNames = movie["category"]?.ToObject<List<JObject>>() ?? new List<JObject>();
                filmData.Actors = new List<Actor>();
                foreach (var act in actorNames)
                {
                    var name1 = act["name"]?.ToString();
                    Actor actor;
                    actor = await _context.Actors.FirstOrDefaultAsync(c => c.Name == name1);
                    if (actor == null)
                    {
                        actor = new Actor
                        {
                            Name = act["name"]?.ToString(),
                        };
                        _context.Actors.Add(actor);
                    }
                    filmData.Actors.Add(actor);
                }
                // Process categories
                var categoryObjs = movie["category"]?.ToObject<List<JObject>>() ?? new List<JObject>();
                filmData.Categories = new List<Category>();
                foreach (var cat in categoryObjs)
                {
                    var slug1 = cat["slug"]?.ToString();
                    Category category;
                    category = await _context.categories.FirstOrDefaultAsync(c => c.Slug == slug1);
                    if (category == null)
                    {
                        category = new Category
                        {
                            Id = cat["id"]?.ToString(),
                            Name = cat["name"]?.ToString(),
                            Slug = slug1
                        };
                        _context.categories.Add(category);
                    }
                    filmData.Categories.Add(category);
                }

                // Process countries
                var countryObjs = movie["country"]?.ToObject<List<JObject>>() ?? new List<JObject>();
                filmData.Countries = new List<Country>();
                foreach (var c in countryObjs)
                {
                    var slug2 = c["slug"]?.ToString();
                    var existingCountry = await _context.Countries.FirstOrDefaultAsync(co => co.Slug == slug2)
                                           ?? new Country
                                           {
                                               Id = Guid.TryParse(c["id"]?.ToString(), out var guid) ? guid : Guid.NewGuid(),
                                               Name = c["name"]?.ToString(),
                                               Slug = slug2
                                           };
                    filmData.Countries.Add(existingCountry);
                }

                // Process episodes
                filmData.Episodes = new List<Episode>();
                foreach (var server in episodes)
                {
                    var serverName = server["server_name"]?.ToString();
                    var serverData = server["server_data"]?.ToObject<List<JObject>>();
                    if (serverData != null)
                    {
                        foreach (var ep in serverData)
                        {
                            filmData.Episodes.Add(new Episode
                            {
                                ServerName = serverName,
                                Name = ep["name"]?.ToString(),
                                Slug = ep["slug"]?.ToString(),
                                Filename = ep["filename"]?.ToString(),
                                LinkEmbed = ep["link_embed"]?.ToString(),
                                LinkM3U8 = ep["link_m3u8"]?.ToString()
                            });
                        }
                    }
                }

                if (existingFilm != null)
                {
                    existingFilm.Name = filmData.Name;
                    existingFilm.Slug = filmData.Slug;
                    existingFilm.OriginName = filmData.OriginName;
                    existingFilm.Content = filmData.Content;
                    existingFilm.Type = filmData.Type;
                    existingFilm.Status = filmData.Status;
                    existingFilm.PosterUrl = filmData.PosterUrl;
                    existingFilm.ThumbUrl = filmData.ThumbUrl;
                    existingFilm.IsCopyright = filmData.IsCopyright;
                    existingFilm.SubDocquyen = filmData.SubDocquyen;
                    existingFilm.ChieuRap = filmData.ChieuRap;
                    existingFilm.TrailerUrl = filmData.TrailerUrl;
                    existingFilm.Time = filmData.Time;
                    existingFilm.EpisodeCurrent = filmData.EpisodeCurrent;
                    existingFilm.EpisodeTotal = filmData.EpisodeTotal;
                    existingFilm.Quality = filmData.Quality;
                    existingFilm.Lang = filmData.Lang;
                    existingFilm.Notify = filmData.Notify;
                    existingFilm.Showtimes = filmData.Showtimes;
                    existingFilm.Year = filmData.Year;
                    existingFilm.View = filmData.View;
                    existingFilm.Created = filmData.Created;
                    existingFilm.Modified = filmData.Modified;
                    existingFilm.Actors = filmData.Actors;
                    existingFilm.Categories = filmData.Categories;
                    existingFilm.Countries = filmData.Countries;
                    existingFilm.Episodes = filmData.Episodes;

                    _context.Films.Update(existingFilm);
                }
                else
                {
                    _context.Films.Add(filmData);
                }

                await _context.SaveChangesAsync();
                after_item = _context.Films.Count();
            }

            return Ok(new
            {
                status = true,
                msg = "Added" + " " + (after_item - curr_item) + " " + "New Films !"
            });
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


