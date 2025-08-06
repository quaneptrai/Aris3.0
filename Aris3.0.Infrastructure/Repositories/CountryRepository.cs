using Aris3._0.Application.Interface.Repositories;
using Aris3._0.Domain.Entities;
using Aris3._0.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aris3._0.Infrastructure.Repositories
{
    public class CountryRepository:Repository<Country>,ICountryRepository
    {
        private readonly ArisDbContext dbContext;

        public CountryRepository(ArisDbContext dbContext):base(dbContext)
        {
            this.dbContext = dbContext;
        }
        public bool ExistsBySlug(string slug)
        {
            var result = dbContext.Countries.FirstOrDefault(c => c.Slug == slug);
            if (result == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task<Country> GetBySlugAsync(string slug)
        {
            var country = await dbContext.Countries.FirstOrDefaultAsync(c => c.Slug == slug);
            if (country == null)
                return null;
            return country;
        }
    }
}
