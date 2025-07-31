using Aris3._0.Infrastructure.Data.Context;
using Aris3._0Fe.Models;
using Microsoft.AspNetCore.Mvc;
namespace Aris3._0Fe.Controllers
{
    public class FilmController : Controller
    {
        private readonly ArisDbContext dbContext;

        public FilmController(ArisDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public IActionResult Index(string id)
        {
            var filmarray = dbContext.Episodes
                                     .OrderBy(f => f.Id)
                                     .Where(y => y.FilmId == id).ToList();
            return View(filmarray); 
        }

    }
}
