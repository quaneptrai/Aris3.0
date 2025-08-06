using Aris3._0.Infrastructure.Data.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Aris3._0Fe.ViewComponents
{
    public class CountryDropdownViewComponent:ViewComponent
    {
        private readonly ArisDbContext dbContext;

        public CountryDropdownViewComponent(ArisDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var countries = await dbContext.Countries.ToListAsync();
            return View(countries);
        }
    }
}
