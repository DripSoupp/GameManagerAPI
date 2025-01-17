using GameManagerAPI.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

public class MongoDBSettings
{
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
}


namespace GameManagerAPI.Services
{
    public class MongoDBService
    {
        private readonly MongoClient _mongoClient;
        private readonly IMongoDatabase _database;

        public MongoDBService(IOptions<MongoDBSettings> settings)
        {
            var connectionString = "mongodb+srv://karovdarin:Darkobarko112@cluster0.oub15.mongodb.net/?retryWrites=true&w=majority&appName=Cluster0";
            var databaseName = "GameManager";
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString), "Connection string cannot be null or empty.");
            }

            _mongoClient = new MongoClient(connectionString);
            _database = _mongoClient.GetDatabase(databaseName);
        }

        public MongoClient GetMongoClient() => _mongoClient;
        public IMongoDatabase GetDatabase(string databaseName) => _database;
    }

}

