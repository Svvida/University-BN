
using Serilog;
using Microsoft.Extensions.Logging;
using Domain.Interfaces;
using Domain.Interfaces.InterfacesBase;
using Domain.Interfaces.Repositories;
using Infrastructure.Data;
using Infrastructure.Repositories.RepositoriesBase;
using Infrastructure.Repositories;
using Infrastructure.Seeding;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using Utilities;
using Infrastructure.Seeding.Bogus;

namespace RestApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            // Configure Serilog
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build())
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.Debug()
                .CreateLogger();

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
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
            .EnableSensitiveDataLogging(false)
            .LogTo(Console.WriteLine, LogLevel.Warning));

            // Register Repositories
            builder.Services.AddScoped(typeof(IAddressRepository<>), typeof(AddressRepository<>));
            builder.Services.AddScoped(typeof(IConsentRepository<>), typeof(ConsentRepository<>));
            builder.Services.AddScoped(typeof(IEducationRepository<>), typeof(EducationRepository<>));
            builder.Services.AddScoped(typeof(IPersonRepository<>), typeof(PersonRepository<>));
            builder.Services.AddScoped<IAccountRepository, AccountRepository>();
            builder.Services.AddScoped<IRoleRepository, RoleRepository>();

            // Register Unit Of Work
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<SeedDataFromFile>();

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
                    RoleSeeding.Initialize(services);

                    var excelFilePaths = configuration.GetSection("SeedData:ExcelFilePaths").Get<List<string>>();
                    var seedDataFromFile = services.GetRequiredService<SeedDataFromFile>();

                    StopwatchService.Instance.Start();
                    Logger.Instance.Log("Seeding database from files");
                    foreach (var filePath in excelFilePaths)
                    {
                        await seedDataFromFile.InitializeAsync(filePath);
                    }
                    StopwatchService.Instance.Stop();
                    StopwatchService.Instance.LogElapsed("Seeding database from file completed", "seconds");

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
