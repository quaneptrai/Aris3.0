using Aris3._0.Application.Interface.Repositories;
using Aris3._0.Application.Interface.Service;
using Aris3._0.Application.Mapping;
using Aris3._0.Application.Service;
using Aris3._0.Infrastructure;
using Aris3._0.Infrastructure.Data.Context;
using Aris3._0.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Aris3._0
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddHttpClient();
            builder.Services.AddControllers();
            builder.Services.AddDbContextPool<ArisDbContext>(options =>
            {
                var b = builder.Configuration.GetConnectionString("DefaultConnection");
                Console.WriteLine(b);
                options.UseSqlServer(b);
            });
            builder.Services.AddAutoMapper(typeof(MappingProfile));
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddHttpClient();
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();
            builder.Services.AddScoped<IFilmService,FilmService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseHttpsRedirection();
            
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
