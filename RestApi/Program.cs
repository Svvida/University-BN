

using Domain.Interfaces.InterfacesBase;
using Infrastructure.Data;
using Infrastructure.Repositories.RepositoriesBase;
using Infrastructure.Seeding;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace RestApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Configure DbContext
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<UniversityContext>(options =>
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

            // Register Repositories
            builder.Services.AddScoped(typeof(IAddressRepository<>), typeof(AddressRepository<>));

            // Register ExcelService
            builder.Services.AddSingleton<ExcelService>();

            var app = builder.Build();

            // Seed database
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var contentRootPath = app.Services.GetRequiredService<IWebHostEnvironment>().ContentRootPath;
                    var excelFilePath = Path.Combine(contentRootPath, "Data", "Informatyka_Stosowana.xlsx");
                    SeedData.Initialize(services, excelFilePath);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }

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
