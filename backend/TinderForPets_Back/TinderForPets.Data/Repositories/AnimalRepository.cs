using Microsoft.EntityFrameworkCore;
using System.Threading;
using TinderForPets.Core.Models;
using TinderForPets.Data.Entities;
using TinderForPets.Data.Exceptions;
using TinderForPets.Data.Interfaces;

namespace TinderForPets.Data.Repositories
{
    public class AnimalRepository(TinderForPetsDbContext context) : TinderForPetsRepository<Animal>(context), IAnimalRepository
    {
        public async override Task<Guid> CreateAsync(Animal animal, CancellationToken cancellationToken)
        {
            await _context.AddAsync(animal, cancellationToken);
            await _context.SaveChangesAsync();
            return animal.Id;
        }

        public async override Task<int> UpdateAsync(Animal animalEntity, CancellationToken cancellationToken)
        {
            var rowsUpdated = await _context.Animals
            .Where(a => a.Id == animalEntity.Id && a.UserId == animalEntity.UserId)
            .ExecuteUpdateAsync(animal =>
                animal
                .SetProperty(a => a.TypeId, animalEntity.TypeId)
                .SetProperty(a => a.BreedId, animalEntity.BreedId),
                cancellationToken
            );

            return rowsUpdated == 0 ? throw new AnimalNotFoundException(animalEntity.Id) : rowsUpdated;

        }
        public async Task<Animal> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await _dbSet
                .Include(a => a.Profile)
                .Include(a => a.User)
                .FirstOrDefaultAsync(a => a.UserId == userId, cancellationToken);
        }
    }
}
