using Aris3._0.Application.DTOs;
using Aris3._0.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aris3._0.Application.Interface.Repositories
{
    public interface IFilmRepository:IRepository<Film>
    {
        Task<Film?> GetBySlug(string slug);
        Task<List<Film>> GetAllAsync();
        Task<int> CountAsync();
    }
}
