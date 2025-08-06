using Aris3._0.Application.DTOs;
using Aris3._0.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aris3._0.Application.Interface.Service
{
    public interface IFilmService
    {
        Task<Film?> ImportFilmFromSlugAsync(string slug);
        Task<int> ImportAllNewUpdatedFilmsAsync(int pages = 3);
        Task<List<Film>> GetAllFromDbAsync();
    }
}
