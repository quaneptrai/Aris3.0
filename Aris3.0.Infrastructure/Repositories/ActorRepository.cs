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
    public class ActorRepository : Repository<Actor>, IActorRepository
    {
        private readonly ArisDbContext dbContext;

        public ActorRepository(ArisDbContext dbContext):base(dbContext) 
        {
            this.dbContext = dbContext;
        }
        public bool ExistsBySlug(string slug)
        {
            throw new NotImplementedException();
        }

        public async Task<Actor> GetByNameAsync(string name)
        {
            var actor = await dbContext.Actors.FirstOrDefaultAsync(c => c.Name == name);
            if (actor == null)
                return null;
            return actor;
        }
    }
}
