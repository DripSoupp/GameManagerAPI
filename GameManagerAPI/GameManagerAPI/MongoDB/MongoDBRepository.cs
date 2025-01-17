using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameManagerAPI.Repositories
{
    public class MongoRepository<T> : IRepository<T> where T : class
    {
        private readonly IMongoCollection<T> _collection;

        public MongoRepository(IMongoClient mongoClient, string databaseName, string collectionName)
        {
            var database = mongoClient.GetDatabase(databaseName);
            _collection = database.GetCollection<T>(collectionName);
        }

        // Fetch all documents from the collection
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        // Fetch a single document by its ID
        public async Task<T> GetByIdAsync(int id)
        {
            var filter = Builders<T>.Filter.Eq("Id", id);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        // Add a new document to the collection
        public async Task AddAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "Entity cannot be null.");
            }

            await _collection.InsertOneAsync(entity);
        }

        // Update an existing document in the collection
        public async Task UpdateAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "Entity cannot be null.");
            }

            var id = entity.GetType().GetProperty("Id")?.GetValue(entity);
            if (id == null)
            {
                throw new ArgumentException("Entity must have an Id property.");
            }

            var filter = Builders<T>.Filter.Eq("Id", id);
            var result = await _collection.ReplaceOneAsync(filter, entity);

            if (result.MatchedCount == 0)
            {
                throw new KeyNotFoundException($"No document found with Id {id}.");
            }
        }

        // Delete a document by its ID
        public async Task DeleteAsync(int id)
        {
            var filter = Builders<T>.Filter.Eq("Id", id);
            var result = await _collection.DeleteOneAsync(filter);

            if (result.DeletedCount == 0)
            {
                throw new KeyNotFoundException($"No document found with Id {id}.");
            }
        }
    }
}
