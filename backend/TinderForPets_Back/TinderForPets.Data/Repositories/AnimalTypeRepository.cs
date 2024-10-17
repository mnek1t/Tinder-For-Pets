using Microsoft.EntityFrameworkCore;
using SharedKernel;
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

        public async Task<Result<List<AnimalType>>> GetAllAnimalTypesAsync() 
        {
            var animalTypes = await _dbContext.AnimalTypes.ToListAsync();
            return Result.Success<List<AnimalType>>(animalTypes);
        }
    }
}
