using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aris3._0.Infrastructure.Data.Context
{
    public class ArisDbContextFactory : IDesignTimeDbContextFactory<ArisDbContext>
    {
        public ArisDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ArisDbContext>();
            optionsBuilder.UseSqlServer("Server=QUANDM1;Database=Aris3.0;Trusted_Connection=True;TrustServerCertificate=True");
            return new ArisDbContext(optionsBuilder.Options);
        }
    }
}
