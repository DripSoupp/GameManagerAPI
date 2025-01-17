using GameManagerAPI.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameManagerAPI.Repositories
{
    public class DeveloperRepository : IRepository<Developer>
    {
        private readonly IMongoCollection<Developer> _developersCollection;

        // Constructor to initialize MongoDB collection
        public DeveloperRepository(IMongoDatabase database)
        {
            _developersCollection = database.GetCollection<Developer>("Developers");
        }

        // Fetch all developers from MongoDB
        public async Task<IEnumerable<Developer>> GetAllAsync()
        {
            return await _developersCollection.Find(_ => true).ToListAsync();
        }

        // Fetch a single developer by ID from MongoDB
        public async Task<Developer> GetByIdAsync(int id)
        {
            return await _developersCollection.Find(d => d.Id == id).FirstOrDefaultAsync();
        }

        // Add a new developer to MongoDB
        public async Task AddAsync(Developer entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "Developer cannot be null.");
            }

            // Ensure the developer has a unique ID
            var existingDeveloper = await _developersCollection.Find(d => d.Id == entity.Id).FirstOrDefaultAsync();
            if (existingDeveloper != null)
            {
                throw new InvalidOperationException($"A developer with ID {entity.Id} already exists.");
            }

            await _developersCollection.InsertOneAsync(entity);
        }

        // Update an existing developer in MongoDB
        public async Task UpdateAsync(Developer entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "Developer cannot be null.");
            }

            var updateResult = await _developersCollection.ReplaceOneAsync(
                d => d.Id == entity.Id,
                entity
            );

            if (updateResult.MatchedCount == 0)
            {
                throw new KeyNotFoundException($"Developer with ID {entity.Id} not found.");
            }
        }

        // Delete a developer by its ID from MongoDB
        public async Task DeleteAsync(int id)
        {
            var deleteResult = await _developersCollection.DeleteOneAsync(d => d.Id == id);
            if (deleteResult.DeletedCount == 0)
            {
                throw new KeyNotFoundException($"Developer with ID {id} not found.");
            }
        }
    }
}
