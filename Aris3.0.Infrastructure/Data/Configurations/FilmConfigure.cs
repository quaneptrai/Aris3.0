using Aris3._0.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Aris3._0.Infrastructure.Data.Configurations
{
    public class FilmConfigure : IEntityTypeConfiguration<Film>
    {
        public void Configure(EntityTypeBuilder<Film> builder)
        {
            builder.HasKey(a => a.Id);
            builder.HasMany(f => f.Categories)
                  .WithMany(c => c.Films)
                  .UsingEntity(j => j.ToTable("CategoryFilm"));
            builder.HasMany(f => f.Actors)
                   .WithMany(a => a.Films)
                   .UsingEntity(j => j.ToTable("ActorFilm"));
            builder.HasMany(f => f.Directors)
                  .WithMany(a => a.Films)
                  .UsingEntity(j => j.ToTable("DirectorFilm"));
            builder.HasMany(f => f.Countries)
                  .WithMany(a => a.Films)
                  .UsingEntity(j => j.ToTable("CountryFilm"));
        }
    }
}
