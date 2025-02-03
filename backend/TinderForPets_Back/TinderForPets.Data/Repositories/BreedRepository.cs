using Microsoft.EntityFrameworkCore;
using TinderForPets.Data.Entities;
using TinderForPets.Data.Exceptions;
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

        public async Task<List<Breed>> GetBreedsAsync(CancellationToken cancellationToken)
        {
            return await _context.Breeds.ToListAsync(cancellationToken);
        }

        public async Task<List<Breed>> GetBreedsByTypeIdAsync(int animalTypeId, CancellationToken cancellationToken)
        {
            var breeds = await _context.Breeds.Where(b => b.AnimalTypeId == animalTypeId).ToListAsync(cancellationToken);
            return breeds.Count > 0 ? breeds : throw new BreedNotFoundException();
        }
    }
}
