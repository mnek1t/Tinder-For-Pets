using Microsoft.EntityFrameworkCore;
using SharedKernel;
using System.Threading;
using TinderForPets.Data.Entities;
using TinderForPets.Data.Interfaces;

namespace TinderForPets.Data.Repositories
{
    public class AnimalTypeRepository : IAnimalTypeRepository
    {
        private readonly TinderForPetsDbContext _dbContext;
        public AnimalTypeRepository(TinderForPetsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<AnimalType>> GetAllAnimalTypesAsync(CancellationToken cancellationToken) 
        {
            var animalTypes = await _dbContext.AnimalTypes.ToListAsync(cancellationToken);
            return animalTypes;
        }
    }
}
