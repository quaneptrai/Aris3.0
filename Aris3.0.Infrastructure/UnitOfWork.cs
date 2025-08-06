using Aris3._0.Application.Interface.Repositories;
using Aris3._0.Infrastructure.Data.Context;
using Aris3._0.Infrastructure.Repositories;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aris3._0.Infrastructure
{

    public class UnitOfWork:IUnitOfWork
    {
        private readonly ArisDbContext _context;
        private readonly IMapper mapper;
        private readonly CategoryRepository _category;
        private readonly FilmRepository _film;
        private readonly CountryRepository _country;
        private readonly DirectorRepository _director;
        private readonly ActorRepository _actor;
        private IDbContextTransaction _transaction;
        public ICategoryRepository Categorys => _category;
        public IFilmRepository Films => _film;

        public ICountryRepository Countrys => _country;
        public IDirectorRepository Directors => _director;
        public IActorRepository Actors => _actor;
        public UnitOfWork(ArisDbContext context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
            _film = new FilmRepository(context);
            _category = new CategoryRepository(context);
            _country = new CountryRepository(context);
            _director = new DirectorRepository(context);
            _actor = new ActorRepository(context);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }
        public async Task CommitTransactionAsync()
        {
            await _transaction.CommitAsync();
        }

        public async Task RollbackAsync()
        {
            await _transaction.RollbackAsync();
        }
    }
}
