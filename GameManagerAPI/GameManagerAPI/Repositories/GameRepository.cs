using GameManagerAPI.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameManagerAPI.Repositories
{
    public class GameRepository : IRepository<Game>
    {
        private readonly IMongoCollection<Game> _gamesCollection;

        public GameRepository(IMongoDatabase database)
        {
            _gamesCollection = database.GetCollection<Game>("Games");
        }

        // Fetch all games from MongoDB
        public async Task<IEnumerable<Game>> GetAllAsync()
        {
            return await _gamesCollection.Find(_ => true).ToListAsync();
        }

        // Fetch a single game by ID from MongoDB
        public async Task<Game> GetByIdAsync(int id)
        {
            return await _gamesCollection.Find(g => g.Id == id).FirstOrDefaultAsync();
        }

        // Add a new game to MongoDB
        public async Task AddAsync(Game game)
        {
            if (game == null)
                throw new ArgumentNullException(nameof(game), "Game cannot be null.");

            // Ensure the game has a unique ID
            var existingGame = await _gamesCollection.Find(g => g.Id == game.Id).FirstOrDefaultAsync();
            if (existingGame != null)
            {
                throw new InvalidOperationException($"A game with ID {game.Id} already exists.");
            }

            await _gamesCollection.InsertOneAsync(game);
        }

        // Update an existing game in MongoDB
        public async Task UpdateAsync(Game game)
        {
            if (game == null)
                throw new ArgumentNullException(nameof(game), "Game cannot be null.");

            var updateResult = await _gamesCollection.ReplaceOneAsync(
                g => g.Id == game.Id,
                game
            );

            if (updateResult.MatchedCount == 0)
            {
                throw new KeyNotFoundException($"Game with ID {game.Id} not found.");
            }
        }

        // Delete a game by its ID from MongoDB
        public async Task DeleteAsync(int id)
        {
            var deleteResult = await _gamesCollection.DeleteOneAsync(g => g.Id == id);
            if (deleteResult.DeletedCount == 0)
            {
                throw new KeyNotFoundException($"Game with ID {id} not found.");
            }
        }
    }
}
