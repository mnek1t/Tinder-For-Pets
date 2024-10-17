using Microsoft.EntityFrameworkCore;
using TinderForPets.Data.Entities;
using TinderForPets.Data.Interfaces;

namespace TinderForPets.Data.Repositories
{
    public class BreedRepository : IBreedRepository
    {
        private readonly TinderForPetsDbContext _context;

        public BreedRepository(TinderForPetsDbContext context)
        {
            _context = context;
        }

        public async Task<List<Breed>> GetBreedsAsync()
        {
            return await _context.Breeds.ToListAsync();
        }

        public async Task<List<Breed>?> GetBreedsByTypeIdAsync(int id)
        {
            var breeds = await _context.Breeds.Where(b => b.AnimalTypeId == id).ToListAsync();
            return breeds.Count > 0 ? breeds : null;
        }
    }
}
