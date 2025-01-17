using GameManagerAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace GameManagerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameController : ControllerBase
    {
        private readonly GameService _gameService;
        private readonly ILogger<GameController> _logger;

        public GameController(GameService gameService, ILogger<GameController> logger)
        {
            _gameService = gameService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllGames()
        {
            try
            {
                var games = await _gameService.GetAllGamesAsync();
                return Ok(games);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all games.");
                return StatusCode(500, new { Message = "An error occurred while retrieving the games." });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetGameById(int id)
        {
            try
            {
                var game = await _gameService.GetGameByIdAsync(id);
                return game != null ? Ok(game) : NotFound(new { Message = "Game not found." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving game with ID {GameId}", id);
                return StatusCode(500, new { Message = "An error occurred while retrieving the game." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddGame([FromBody] Game game)
        {
            if (game == null)
            {
                return BadRequest("Game data cannot be null.");
            }

            try
            {
                await _gameService.AddGameAsync(game);
                return CreatedAtAction(nameof(GetGameById), new { id = game.Id }, game);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding game.");
                return StatusCode(500, new { Message = "An error occurred while adding the game." });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGame(int id, [FromBody] Game game)
        {
            if (game == null || id != game.Id)
            {
                return BadRequest("Game data is invalid or ID mismatch.");
            }

            try
            {
                var existingGame = await _gameService.GetGameByIdAsync(id);
                if (existingGame == null)
                {
                    return NotFound(new { Message = "Game not found." });
                }

                await _gameService.UpdateGameAsync(game);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating game with ID {GameId}", id);
                return StatusCode(500, new { Message = "An error occurred while updating the game." });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGame(int id)
        {
            try
            {
                var existingGame = await _gameService.GetGameByIdAsync(id); 
                if (existingGame == null)
                {
                    return NotFound(new { Message = "Game not found." });
                }

                await _gameService.DeleteGameAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting game with ID {GameId}", id);
                return StatusCode(500, new { Message = "An error occurred while deleting the game." });
            }
        }
    }
}
