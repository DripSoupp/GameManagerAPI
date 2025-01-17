using GameManagerAPI.Models;
using GameManagerAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class GameService
{
    private readonly IRepository<Game> _gameRepository;

    public GameService(IRepository<Game> gameRepository)
    {
        _gameRepository = gameRepository;
    }

    // Asynchronous method to get all games from MongoDB
    public async Task<IEnumerable<Game>> GetAllGamesAsync()
    {
        return await _gameRepository.GetAllAsync();
    }

    // Asynchronous method to get a single game by its Id from MongoDB
    public async Task<Game> GetGameByIdAsync(int id)
    {
        return await _gameRepository.GetByIdAsync(id);
    }

    // Asynchronous method to add a new game to MongoDB
    public async Task AddGameAsync(Game game)
    {
        if (game == null)
        {
            throw new ArgumentNullException(nameof(game), "Game cannot be null.");
        }

        // Check if the ID is 0, indicating it has not been assigned
        if (game.Id == 0)
        {
            // Generate a new non-zero ID (adjust the logic as needed)
            game.Id = GenerateNewId();
        }

        // Validate other game properties
        if (string.IsNullOrEmpty(game.Title))
        {
            throw new ArgumentException("Game title is required.");
        }

        if (string.IsNullOrEmpty(game.Genre))
        {
            throw new ArgumentException("Game genre is required.");
        }

        await _gameRepository.AddAsync(game);
    }

    // Simple method to generate a new unique ID
    private int GenerateNewId()
    {
        return new Random().Next(1, int.MaxValue); // Example random generator for new IDs
    }

    // Asynchronous method to update an existing game in MongoDB
    public async Task UpdateGameAsync(Game game)
    {
        if (game == null)
        {
            throw new ArgumentNullException(nameof(game), "Game cannot be null.");
        }

        // Validate game properties
        if (game.Id <= 0)
        {
            throw new ArgumentException("Game ID must be a positive integer.");
        }

        if (string.IsNullOrEmpty(game.Title))
        {
            throw new ArgumentException("Game title is required.");
        }

        if (string.IsNullOrEmpty(game.Genre))
        {
            throw new ArgumentException("Game genre is required.");
        }

        await _gameRepository.UpdateAsync(game);
    }

    // Asynchronous method to delete a game by its Id from MongoDB
    public async Task DeleteGameAsync(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Game ID must be a positive integer.");
        }

        var game = await _gameRepository.GetByIdAsync(id);
        if (game == null)
        {
            throw new KeyNotFoundException($"Game with ID {id} not found.");
        }

        await _gameRepository.DeleteAsync(id);
    }
}
