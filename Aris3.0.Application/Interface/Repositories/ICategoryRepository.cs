using Aris3._0.Application.DTOs;
using Aris3._0.Domain.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aris3._0.Application.Interface.Repositories
{
    public interface ICategoryRepository:IRepository<Category>
    {
        Task<Category> GetBySlugAsync(string slug);
        bool ExistsBySlug(string slug);
    }
}
