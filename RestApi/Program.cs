

using Domain.Interfaces.InterfacesBase;
using Infrastructure.Data;
using Infrastructure.Repositories.RepositoriesBase;
using Infrastructure.Seeding;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using Utilities;

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

            // Add configuration
            var configuration = builder.Configuration;

            // Configure DbContext
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<UniversityContext>(options =>
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

            // Register Repositories
            builder.Services.AddScoped(typeof(IAddressRepository<>), typeof(AddressRepository<>));

            // Register ExcelService
            builder.Services.AddSingleton<ExcelService>();

            // Pass EPPlus configuration to Infrastucture layer
            ExcelPackage.LicenseContext = configuration.GetSection("EPPlus:ExcelPackage").Get<LicenseContext>();

            var app = builder.Build();

            // Initialize Logger
            var logger = app.Services.GetRequiredService<ILogger<Logger>>();
            Logger.Instance.SetLogger(logger);

            // Seed database
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var excelFilePaths = configuration.GetSection("SeedData:ExcelFilePaths").Get<List<string>>();
                    foreach (var filePath in excelFilePaths)
                    {
                        SeedDataFromFile.Initialize(services, filePath);
                    }

                    BogusSeeder.Initialize(services);
                }
                catch (Exception ex)
                {
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
