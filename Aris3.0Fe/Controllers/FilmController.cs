using Aris3._0.Infrastructure.Data.Context;
using Aris3._0Fe.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text;
namespace Aris3._0Fe.Controllers
{
    public class FilmController : Controller
    {
        private readonly ArisDbContext dbContext;

        public FilmController(ArisDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public IActionResult Index(string id, string posterurl, string name, string original_name)
        {
            var episodesArray = new List<Episode>();
            if (id != null)
            {
                episodesArray = dbContext.Episodes
                               .Include(e => e.Film)
                               .ThenInclude(f => f.Categories)
                               .Where(e => e.FilmId == id)
                               .OrderBy(e => e.Id)
                               .ToList();
            }
            return View(episodesArray);
        }


        [HttpGet]
        public async Task<IActionResult> Search(string searchQuery)
        {
            // Helper to normalize and remove accents
            string RemoveDiacritics(string text)
            {
                if (string.IsNullOrEmpty(text)) return text;
                var normalized = text.Normalize(NormalizationForm.FormD);
                var sb = new StringBuilder();
                foreach (var c in normalized)
                {
                    if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                        sb.Append(c);
                }
                return sb.ToString().Normalize(NormalizationForm.FormC).ToLower();
            }

            List<Film> films = await dbContext.Films.ToListAsync();

            List<Film> key;

            if (!string.IsNullOrWhiteSpace(searchQuery) && searchQuery.Length >= 3)
            {
                string keywordStart = RemoveDiacritics(searchQuery.Substring(0, 3));

                key = films.Where(f => !string.IsNullOrEmpty(f.Name) &&
                                       RemoveDiacritics(f.Name).StartsWith(keywordStart))
                           .ToList();

                if (key.Count >= 1)
                {
                    ViewBag.Count = key.Count;
                    ViewBag.TuKhoa = searchQuery;
                    return View(key);
                }
            }
            else
            {
                key = films;
                ViewBag.Count = key.Count;
                ViewBag.TuKhoa = searchQuery;
                return View(key);
            }

            return NotFound("Không tìm thấy phim.");
        }
    }
}


