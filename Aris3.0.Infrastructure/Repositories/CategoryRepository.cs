using Aris3._0.Application.DTOs;
using Aris3._0.Application.Interface.Repositories;
using Aris3._0.Domain.Entities;
using Aris3._0.Infrastructure.Data.Context;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Aris3._0.Infrastructure.Repositories
{
    public class CategoryRepository : Repository<Category>,ICategoryRepository
    {
        private readonly ArisDbContext context;

        public CategoryRepository(ArisDbContext _context):base(_context) 
        {
            context = _context;
        }
        public bool ExistsBySlug(string slug)
        {
           var result = context.categories.FirstOrDefault(c => c.Slug == slug);
            if(result == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task<Category> GetBySlugAsync(string slug)
        {
            var category = await context.categories
                               .FirstOrDefaultAsync(c => c.Slug == slug);

            if (category == null)
                return null;
            return category;
        }

    }
}
