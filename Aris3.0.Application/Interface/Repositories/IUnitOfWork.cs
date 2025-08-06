using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aris3._0.Application.Interface.Repositories
{
    public interface IUnitOfWork:IDisposable
    {
        ICategoryRepository Categorys { get; }
        IFilmRepository Films { get; }
        ICountryRepository Countrys { get; }
        IDirectorRepository Directors { get; }
        IActorRepository Actors { get; }
        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackAsync();
    }
}
