using GameManagerAPI.Models;
using GameManagerAPI.Repositories;
using GameManagerAPI.Services;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace GameManagerAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container
            builder.Services.AddControllers();

            // Load MongoDB configuration from appsettings.json
            builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("MongoDB"));

            // Register the MongoDBService to handle client and database access
            builder.Services.AddSingleton<MongoDBService>();

            // Register repositories with dependency injection
            builder.Services.AddScoped<IRepository<Game>>(sp =>
            {
                var mongoService = sp.GetRequiredService<MongoDBService>();
                return new MongoRepository<Game>(mongoService.GetMongoClient(), mongoService.GetDatabase("GameManager").DatabaseNamespace.DatabaseName, "Games");
            });

            builder.Services.AddScoped<IRepository<Developer>>(sp =>
            {
                var mongoService = sp.GetRequiredService<MongoDBService>();
                return new MongoRepository<Developer>(mongoService.GetMongoClient(), mongoService.GetDatabase("GameManager").DatabaseNamespace.DatabaseName, "Developers");
            });

            // Register domain-specific services
            builder.Services.AddScoped<GameService>();
            builder.Services.AddScoped<DeveloperService>();

            // Add Swagger for API documentation
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Add Health Checks (Optional but useful for monitoring)
            builder.Services.AddHealthChecks();

            var app = builder.Build();

            // Configure the HTTP request pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();

            // Map controllers
            app.MapControllers();

            // Map Health Checks endpoint (Optional)
            app.MapHealthChecks("/healthz");

            // Run the application
            app.Run();
        }
    }
}

     