using Aris3._0.Application.Interface.Repositories;
using Aris3._0.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Aris3._0.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ArisDbContext context;

        public Repository(ArisDbContext _context)
        {
            context = _context;
        }
        public async Task AddAsync(T entity)
        {
           await context.Set<T>().AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await context.Set<T>().AddRangeAsync(entities);
        }

        public async Task<bool> Any(Expression<Func<T, bool>> predicate)
        {
            var result = await context.Set<T>().Where(predicate).FirstOrDefaultAsync();
            if (result != null) { return true; }
            else { return false; }
        }

        public Task DeleteAsync(T entity)
        {
            context.Set<T>().Remove(entity);
            return Task.CompletedTask;
        }

        public async Task DeleteEntityRelationship(T entity)
        {
            var findEntity = await context.Set<T>().Include(e => e).FirstOrDefaultAsync(e => e.Equals(entity));
            if (findEntity != null)
            {
                context.Set<T>().Remove(findEntity);
                await context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Entity not found");
            }
        }

        public Task DeleteRangeAsync(IEnumerable<T> entities)
        {
            context.Set<T>().RemoveRange(entities);
            return Task.CompletedTask;
        }

        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
        {
            return await context.Set<T>().Where(predicate).AnyAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await context.Set<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllConditionAsync(Expression<Func<T, bool>> predicate)
        {
            return await context.Set<T>().Where(predicate).ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await context.Set<T>().FindAsync(id);
        }

        public async Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await context.Set<T>().Where(predicate).FirstOrDefaultAsync();
        }

        public Task UpdateAsync(T entity)
        {
            context.Set<T>().Update(entity);
            return Task.CompletedTask;
        }

        public async Task UpdateRangeAsync(IEnumerable<T> entities)
        {
            context.Set<T>().UpdateRange(entities);
            await context.SaveChangesAsync();
        }

    }
}
