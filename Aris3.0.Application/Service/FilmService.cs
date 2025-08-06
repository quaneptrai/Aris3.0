using Aris3._0.Application.Interface.Repositories;
using Aris3._0.Application.Interface.Service;
using Aris3._0.Domain.Entities;
using AutoMapper;
using Azure;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Aris3._0.Application.Service
{
    public class FilmService : IFilmService
    {
        private readonly HttpClient _client;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper mapper;
        private readonly IFilmRepository _filmRepository;

        public FilmService(HttpClient client, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _client = client;
            _unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<Film?> ImportFilmFromSlugAsync(string slug)
        {
            var response = await _client.GetAsync($"https://phimapi.com/phim/{slug}");
            var content = await response.Content.ReadAsStringAsync();
            var jObj = JObject.Parse(content);
            var movie = jObj["movie"];
            var tmbdId = movie["tmdb"]?["id"]?.ToString();
            if (string.IsNullOrWhiteSpace(tmbdId))
{
    Console.WriteLine($"Skipping {slug}: TMDB ID is null.");
    return null;
}
            var episodes = jObj["episodes"];
            var filmRepo = _unitOfWork.Films;
            var existingFilm = await filmRepo.GetBySlug(slug); 
            var filmData = movie.ToObject<Film>();
            // Process categories
            filmData.Categories = await ProcessCategories(movie);

            // Process countries
            filmData.Countries = await ProcessCountries(movie);

            // Process episodes
            filmData.Episodes = ProcessEpisodes(episodes);

            // Process directors
            filmData.Directors = await ProcessDirectors(movie);

            // Process actors
            filmData.Actors = await ProcessActors(movie);
            if (existingFilm != null)
            {
                UpdateExistingFilm(existingFilm, filmData);
                _unitOfWork.Films.UpdateAsync(existingFilm); 
            }
            else
            {
                await filmRepo.AddAsync(filmData);
            }
            await _unitOfWork.SaveChangesAsync();
            return filmData;
        }

        private async Task<List<Category>> ProcessCategories(JToken movie)
        {
            var categoryObjs = movie["category"]?.ToObject<List<JObject>>() ?? new();
            var repo = _unitOfWork.Categorys;
            var categories = new List<Category>();

            foreach (var cat in categoryObjs)
            {
                var slug = cat["slug"]?.ToString();
                var category = await repo.GetBySlugAsync(slug);
                if (category == null)
                {
                    category = new Category
                    {
                        Id = cat["id"]?.ToString(),
                        Name = cat["name"]?.ToString(),
                        Slug = slug
                    };
                    await repo.AddAsync(category);
                }
                categories.Add(category);
            }
            return categories;
        }

        private async Task<List<Country>> ProcessCountries(JToken movie)
        {
            var countryObjs = movie["country"]?.ToObject<List<JObject>>() ?? new();
            var repo = _unitOfWork.Countrys;
            var countries = new List<Country>();
            foreach (var coun in countryObjs)
            {
                var slug = coun["slug"]?.ToString();
                var country = await repo.GetBySlugAsync(slug);
                if (country == null)
                {
                    country = new Country
                    {
                        Id = Guid.TryParse(coun["id"]?.ToString(), out var guid) ? guid : Guid.NewGuid(),
                        Name = coun["name"]?.ToString(),
                        Slug = slug
                    };
                }       
                countries.Add(country);
            }
            return countries;
        }

        private List<Episode> ProcessEpisodes(JToken episodes)
        {
            var result = new List<Episode>();
            foreach (var server in episodes ?? new JArray())
            {
                var serverName = server["server_name"]?.ToString();
                var serverData = server["server_data"]?.ToObject<List<JObject>>();
                if (serverData != null)
                {
                    result.AddRange(serverData.Select(ep => new Episode
                    {
                        ServerName = serverName,
                        Name = ep["name"]?.ToString(),
                        Slug = ep["slug"]?.ToString(),
                        Filename = ep["filename"]?.ToString(),
                        LinkEmbed = ep["link_embed"]?.ToString(),
                        LinkM3U8 = ep["link_m3u8"]?.ToString()
                    }));
                }
            }
            return result;
        }

        private async Task<List<Director>> ProcessDirectors(JToken movie)
        {
            var directorNames = movie["director"]?.ToObject<List<string>>() ?? new();
            var repo = _unitOfWork.Directors;
            var directors = new List<Director>();

            foreach (var dirName in directorNames)
            {
                var director = await repo.GetByNameAsync(dirName);
                if (director == null)
                {
                    director = new Director
                    {
                        Name = dirName
                    };
                    await repo.AddAsync(director);
                }
                directors.Add(director);
            }

            return directors;
        }


        private async Task<List<Actor>> ProcessActors(JToken movie)
        {
            var directorNames = movie["actor"]?.ToObject<List<string>>() ?? new();
            var repo = _unitOfWork.Actors;
            var actors = new List<Actor>();

            foreach (var dirName in directorNames)
            {
                var actor = await repo.GetByNameAsync(dirName);
                if (actor == null)
                {
                    actor = new Actor
                    {
                        Name= dirName
                    };
                    await repo.AddAsync(actor);
                }
                actors.Add(actor);
            }

            return actors;
        }

        private void UpdateExistingFilm(Film existing, Film source)
        {
            existing.Name = source.Name;
            existing.Slug = source.Slug;
            existing.OriginName = source.OriginName;
            existing.Content = source.Content;
            existing.Type = source.Type;
            existing.Status = source.Status;
            existing.PosterUrl = source.PosterUrl;
            existing.ThumbUrl = source.ThumbUrl;
            existing.IsCopyright = source.IsCopyright;
            existing.SubDocquyen = source.SubDocquyen;
            existing.ChieuRap = source.ChieuRap;
            existing.TrailerUrl = source.TrailerUrl;
            existing.Time = source.Time;
            existing.EpisodeCurrent = source.EpisodeCurrent;
            existing.EpisodeTotal = source.EpisodeTotal;
            existing.Quality = source.Quality;
            existing.Lang = source.Lang;
            existing.Notify = source.Notify;
            existing.Showtimes = source.Showtimes;
            existing.Year = source.Year;
            existing.View = source.View;
            existing.Created = source.Created;
            existing.Modified = source.Modified;
            existing.Actors = source.Actors;
            existing.Categories = source.Categories;
            existing.Countries = source.Countries;
            existing.Episodes = source.Episodes;
        }

        public async Task<int> ImportAllNewUpdatedFilmsAsync(int pages)
        {
            List<string> slugs = new List<string>();
            for (int i = 0; i < pages; i++) {
                var pageSlugs = await GetSLugFromApi(i); 
                slugs.AddRange(pageSlugs);
            }
            foreach (var slug in slugs)
            {
                await ImportFilmFromSlugAsync(slug);
            }
            return slugs.Count;
        }
        public async Task<List<string>> GetSLugFromApi(int i)
        {
            List<string> slugs = new List<string>();
            var response = await _client.GetAsync($"https://phimapi.com/danh-sach/phim-moi-cap-nhat-v2?page={i}");
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
            return slugs;
        }
        public async Task<List<Film>> GetAllFromDbAsync()
        {
            throw new NotImplementedException();
        }
    }
}

