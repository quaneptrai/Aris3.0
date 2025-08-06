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
    public class DirectorRepository:Repository<Director>,IDirectorRepository
    {
        private readonly ArisDbContext dbContext;
        public DirectorRepository(ArisDbContext dbContext):base(dbContext) 
        {
            this.dbContext = dbContext;
        }
        public bool ExistsBySlug(string name)
        {
            var result = dbContext.Directors.FirstOrDefault(c => c.Name == name);
            if (result == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task<Director> GetByNameAsync(string name)
        {
            var director = await dbContext.Directors.FirstOrDefaultAsync(c => c.Name == name);
            if (director == null)
                return null;
            return director;
        }
    }
}
