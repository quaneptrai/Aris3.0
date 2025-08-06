using Aris3._0.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aris3._0.Application.Interface.Repositories
{
    public interface IActorRepository:IRepository<Actor>
    {
        Task<Actor> GetByNameAsync(string slug);
        bool ExistsBySlug(string slug);
    }
}
