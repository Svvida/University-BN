
using Application.Interfaces;
using Application.Services;
using Domain.Entities.AccountEntities;
using Domain.Interfaces;
using Domain.Interfaces.InterfacesBase;
using Domain.Interfaces.Repositories;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Infrastructure.Repositories.RepositoriesBase;
using Infrastructure.Seeding;
using Infrastructure.Seeding.Bogus;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using Serilog;
using Utilities;

namespace RestApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            ConfigureLogger();

            var builder = WebApplication.CreateBuilder(args);
            ConfigureServices(builder);

            var app = builder.Build();

            await SeedDatabaseAsync(app);

            ConfigureMiddleware(app);

            app.Run();
        }

        private static void ConfigureLogger()
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build())
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.Debug()
                .CreateLogger();
        }

        private static void ConfigureServices(WebApplicationBuilder builder)
        {
            // Add services to the container
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Add configuration
            var configuration = builder.Configuration;

            // Configure DbContext
            var connextionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<UniversityContext>(options =>
                options.UseMySql(connextionString, ServerVersion.AutoDetect(connextionString))
                .EnableSensitiveDataLogging(false)
                .LogTo(Console.WriteLine, LogLevel.Warning));

            // Register Repositories
            builder.Services.AddScoped(typeof(IAddressRepository<>), typeof(AddressRepository<>));
            builder.Services.AddScoped(typeof(IConsentRepository<>), typeof(ConsentRepository<>));
            builder.Services.AddScoped(typeof(IEducationRepository<>), typeof(EducationRepository<>));
            builder.Services.AddScoped(typeof(IPersonRepository<>), typeof(PersonRepository<>));
            builder.Services.AddScoped<IAccountRepository, AccountRepository>();
            builder.Services.AddScoped<IRoleRepository, RoleRepository>();

            // Register Services
            builder.Services.AddScoped<IAccountService, AccountService>();

            // Register password hasher
            builder.Services.AddSingleton<IPasswordHasher<UserAccount>, PasswordHasher<UserAccount>>();

            // Register Logger
            builder.Services.AddLogging();

            // Register stopwatch service
            builder.Services.AddScoped<StopwatchService>();

            // Register BogusSeeder
            builder.Services.AddScoped<BogusSeeder>();

            // Register Unit Of Work
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Register SeedDataFromFile
            builder.Services.AddScoped<SeedDataFromFile>();

            // Register ExcelService
            builder.Services.AddScoped<ExcelService>();

            // Pass EPPlus configuration to Infrastucture layer
            ExcelPackage.LicenseContext = configuration.GetSection("EPPlus:ExcelPackage").Get<LicenseContext>();
        }

        private static async Task SeedDatabaseAsync(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var configuration = services.GetRequiredService<IConfiguration>();

                try
                {
                    RoleSeeding.Initialize(services);

                    var excelFilePaths = configuration.GetSection("SeedData:ExcelFilePaths").Get<List<string>>();
                    var seedDataFromFile = services.GetRequiredService<SeedDataFromFile>();

                    using (var stopwatchServiceScope = scope.ServiceProvider.CreateScope())
                    {
                        var stopwatchService = stopwatchServiceScope.ServiceProvider.GetRequiredService<StopwatchService>();

                        stopwatchService.Start();

                        foreach(var filePath in excelFilePaths)
                        {
                            await seedDataFromFile.InitializeAsync(filePath);
                        }

                        stopwatchService.Stop();
                        stopwatchService.LogElapsed("Seeding database from file completed", "seconds");

                        stopwatchService.Start();
                        var bogusSeeder = services.GetRequiredService<BogusSeeder>();
                        bogusSeeder.Initialize(services);
                        stopwatchService.Stop();
                        stopwatchService.LogElapsed("Bogus seeding completed", "seconds");
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("An error occurred while seeding the database", ex);
                }
            }
        }


        private static void ConfigureMiddleware(WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
        }
    }
}
