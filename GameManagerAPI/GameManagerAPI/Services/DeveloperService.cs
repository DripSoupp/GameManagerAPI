using GameManagerAPI.Models;
using GameManagerAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameManagerAPI.Services
{
    public class DeveloperService
    {
        private readonly IRepository<Developer> _developerRepository;

        public DeveloperService(IRepository<Developer> developerRepository)
        {
            _developerRepository = developerRepository;
        }

        // Asynchronous method to get all developers
        public async Task<IEnumerable<Developer>> GetAllDevelopersAsync() => await _developerRepository.GetAllAsync();

        // Asynchronous method to get a developer by ID
        public async Task<Developer> GetDeveloperByIdAsync(int id)
        {
            var developer = await _developerRepository.GetByIdAsync(id);
            if (developer == null)
            {
                // Optionally throw an exception if developer not found
                throw new KeyNotFoundException($"Developer with ID {id} not found.");
            }
            return developer;
        }

        // Asynchronous method to add a new developer
        public async Task AddDeveloperAsync(Developer developer)
        {
            if (developer == null)
            {
                throw new ArgumentNullException(nameof(developer), "Developer cannot be null.");
            }
            await _developerRepository.AddAsync(developer);
        }

        // Asynchronous method to update an existing developer
        public async Task UpdateDeveloperAsync(Developer developer)
        {
            if (developer == null)
            {
                throw new ArgumentNullException(nameof(developer), "Developer cannot be null.");
            }
            await _developerRepository.UpdateAsync(developer);
        }

        // Asynchronous method to delete a developer by ID
        public async Task DeleteDeveloperAsync(int id)
        {
            var developer = await _developerRepository.GetByIdAsync(id);
            if (developer == null)
            {
                throw new KeyNotFoundException($"Developer with ID {id} not found.");
            }
            await _developerRepository.DeleteAsync(id);
        }
    }
}
