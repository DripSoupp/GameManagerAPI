using GameManagerAPI.Services;
using GameManagerAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace GameManagerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeveloperController : ControllerBase
    {
        private readonly DeveloperService _developerService;
        private readonly ILogger<DeveloperController> _logger;

        public DeveloperController(DeveloperService developerService, ILogger<DeveloperController> logger)
        {
            _developerService = developerService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> AddDeveloper([FromBody] Developer developer)
        {
            if (developer == null)
            {
                return BadRequest("Developer cannot be null.");
            }

            try
            {
                await _developerService.AddDeveloperAsync(developer); 
                return CreatedAtAction(nameof(GetDeveloperById), new { id = developer.Id }, developer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding developer");
                return StatusCode(500, new { Message = "An error occurred while adding the developer." });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDevelopers()
        {
            try
            {
                var developers = await _developerService.GetAllDevelopersAsync();
                return Ok(developers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all developers");
                return StatusCode(500, new { Message = "An error occurred while retrieving the developers." });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDeveloperById(int id)
        {
            try
            {
                var developer = await _developerService.GetDeveloperByIdAsync(id); 
                return developer != null ? Ok(developer) : NotFound(new { Message = "Developer not found." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving developer with ID {DeveloperId}", id);
                return StatusCode(500, new { Message = "An error occurred while retrieving the developer." });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDeveloper(int id, [FromBody] Developer developer)
        {
            if (developer == null)
            {
                return BadRequest("Developer data cannot be null.");
            }

            if (id != developer.Id)
            {
                return BadRequest("Developer ID mismatch.");
            }

            try
            {
                var existingDeveloper = await _developerService.GetDeveloperByIdAsync(id); 
                if (existingDeveloper == null)
                {
                    return NotFound(new { Message = "Developer not found." });
                }

                await _developerService.UpdateDeveloperAsync(developer); 
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating developer with ID {DeveloperId}", id);
                return StatusCode(500, new { Message = "An error occurred while updating the developer." });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDeveloper(int id)
        {
            try
            {
                var existingDeveloper = await _developerService.GetDeveloperByIdAsync(id); 
                if (existingDeveloper == null)
                {
                    return NotFound(new { Message = "Developer not found." });
                }

                await _developerService.DeleteDeveloperAsync(id); 
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting developer with ID {DeveloperId}", id);
                return StatusCode(500, new { Message = "An error occurred while deleting the developer." });
            }
        }
    }
}
