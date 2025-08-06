using Aris3._0.Application.DTOs;
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
    public class FilmRepository : Repository<Film>, IFilmRepository
    {
        private readonly ArisDbContext _context;

        public FilmRepository(ArisDbContext context):base(context) 
        {
            _context = context;
        }

        public async Task<List<Film>> GetAllAsync()
        {
            return await _context.Films
                .Include(f => f.Actors)
                .Include(f => f.Categories)
                .Include(f => f.Countries)
                .ToListAsync();
        }

        public async Task AddAsync(Film film)
        {
            await _context.Films.AddAsync(film);
        }

        public async Task UpdateAsync(Film film)
        {
            _context.Films.Update(film);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<int> CountAsync()
        {
            return await _context.Films.CountAsync();
        }

        public async Task<Film?> GetBySlug(string slug)
        {
            return await _context.Films
                .Include(f => f.Actors)
                .Include(f => f.Categories)
                .Include(f => f.Countries)
                .Include(f => f.Episodes)
                .FirstOrDefaultAsync(f => f.Slug == slug);
        }
    }
}
