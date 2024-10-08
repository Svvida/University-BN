using Application.Interfaces;
using Application.Mappers;
using Application.Services;
using Domain.Entities.AccountEntities;
using Domain.Entities.EmployeeEntities;
using Domain.Entities.EventEntities;
using Domain.Entities.StudentEntities;
using Domain.Interfaces;
using Domain.Interfaces.Base;
using Domain.Interfaces.InterfacesBase;
using Domain.Interfaces.Repositories;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Infrastructure.Repositories.RepositoriesBase;
using Infrastructure.Seeding;
using Infrastructure.Seeding.Bogus;
using Infrastructure.Services;
using Infrastructure.Services.MongoLogging;
using Infrastructure.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using OfficeOpenXml;
using RestApi.Middlewares;
using Serilog;
using System.Text;
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

            //await SeedDatabaseAsync(app);

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

            // Add Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "My Api",
                    Version = "v1"
                });

                // Define the security scheme for JWT Bearer
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter your JWT token in the text input below.\r\n\r\nYou do not need to include 'Bearer ' before the token as it will be added automatically."
                });

                // Require JWT Bearer token for accessing secure endpoints
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
{                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()}
                });
            });

            // Add configuration
            var configuration = builder.Configuration;

            // Add environment variables
            builder.Configuration.AddEnvironmentVariables();

            // Configure DbContext
            var connextionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<UniversityContext>(options =>
                options.UseMySql(connextionString, ServerVersion.AutoDetect(connextionString))
                .EnableSensitiveDataLogging(false)
                .LogTo(Console.WriteLine, LogLevel.Warning),
                ServiceLifetime.Scoped);

            // Add configuration for SmtpSettings
            builder.Services.Configure<SmtpSettings>(configuration.GetSection("SmtpSettings"));

            // Register Infrastructure Services
            builder.Services.AddScoped(typeof(IAddressRepository<>), typeof(AddressRepository<>));
            builder.Services.AddScoped(typeof(IConsentRepository<>), typeof(ConsentRepository<>));
            builder.Services.AddScoped(typeof(IEducationRepository<>), typeof(EducationRepository<>));
            builder.Services.AddScoped(typeof(IPersonRepository<>), typeof(PersonRepository<>));
            builder.Services.AddScoped<IAccountRepository, AccountRepository>();
            builder.Services.AddScoped<IRoleRepository, RoleRepository>();
            builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
            builder.Services.AddScoped<IJwtService, JwtService>();
            builder.Services.AddScoped<HttpJwtService>();
            builder.Services.AddScoped<ICRUDRepository<Employee>, CRUDRepository<Employee>>();
            builder.Services.AddScoped<ICRUDRepository<Student>, CRUDRepository<Student>>();
            builder.Services.AddScoped<ICRUDRepository<Event>, CRUDRepository<Event>>();
            builder.Services.AddTransient<IEmailService, MailKitEmailService>();
            builder.Services.AddTransient<IPasswordResetService, PasswordResetService>();
            builder.Services.AddSingleton<IPasswordResetTokenStore, InMemoryPasswordResetTokenStore>();
            builder.Services.AddSingleton<ITokenManager>(provider =>
            {
                var jwtService = provider.CreateScope().ServiceProvider.GetRequiredService<IJwtService>();
                var logger = provider.GetRequiredService<ILogger<TokenManager>>();
                var accountRepository = provider.CreateScope().ServiceProvider.GetRequiredService<IAccountRepository>();
                return new TokenManager(jwtService, logger, accountRepository);
            });
            builder.Services.AddScoped<IMongoLogger, MongoDbLoggerService>();

            // Register Application Services
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<ILoginService, LoginService>();
            builder.Services.AddScoped<IPersonService<Student>, PersonService<Student>>();
            builder.Services.AddScoped<IPersonService<Employee>, PersonService<Employee>>();

            // Register password hasher
            builder.Services.AddSingleton<IPasswordHasher<UserAccount>, PasswordHasher<UserAccount>>();

            // Register AutoMapper
            builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

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

            // Configure JWT Authentication
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
                };
            });

            // Pass EPPlus configuration to Infrastucture layer
            ExcelPackage.LicenseContext = configuration.GetSection("EPPlus:ExcelPackage").Get<LicenseContext>();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:5173")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                    });
            });

            // Add MongoDB client and database
            builder.Services.AddSingleton<IMongoClient, MongoClient>(sp =>
            {
                var mongoDbConnectionString = configuration.GetConnectionString("MongoDb");
                return new MongoClient(mongoDbConnectionString);
            });

            builder.Services.AddScoped<IMongoDatabase>(sp =>
            {
                var client = sp.GetRequiredService<IMongoClient>();
                return client.GetDatabase("UniversityLogs");
            });
        }

        private static async Task SeedDatabaseAsync(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var configuration = services.GetRequiredService<IConfiguration>();

                try
                {
                    SeedConstants.Initialize(services);

                    var excelFilePaths = configuration.GetSection("SeedData:ExcelFilePaths").Get<List<string>>();
                    var seedDataFromFile = services.GetRequiredService<SeedDataFromFile>();

                    using (var stopwatchServiceScope = scope.ServiceProvider.CreateScope())
                    {
                        var stopwatchService = stopwatchServiceScope.ServiceProvider.GetRequiredService<StopwatchService>();

                        stopwatchService.Start();

                        foreach (var filePath in excelFilePaths)
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
            else
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your api v1");
                    c.RoutePrefix = string.Empty;
                });
            }
            app.UseMiddleware<SessionActivityMiddleware>();
            app.UseHttpsRedirection();
            app.UseCors("AllowFrontend");
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
        }
    }
}
