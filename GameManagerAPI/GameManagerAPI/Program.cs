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

            builder.Services.AddControllers();
            
            builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("MongoDB"));
            
            builder.Services.AddSingleton<MongoDBService>();

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

            builder.Services.AddScoped<GameService>();
            builder.Services.AddScoped<DeveloperService>();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddHealthChecks();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();

            app.MapControllers();

            app.MapHealthChecks("/healthz");

            app.Run();
        }
    }
}

     
