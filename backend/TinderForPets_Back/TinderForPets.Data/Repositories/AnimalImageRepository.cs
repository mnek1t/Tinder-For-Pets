using AutoMapper;
using TinderForPets.Core.Models;
using TinderForPets.Data.Entities;
using TinderForPets.Data.Interfaces;

namespace TinderForPets.Data.Repositories
{
    public class AnimalImageRepository : IAnimalImageRepository
    {
        private readonly TinderForPetsDbContext _dbContext;
        private readonly IMapper _mapper;
        public AnimalImageRepository(TinderForPetsDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<List<AnimalImage>> SaveAnimalMediaAsync(IEnumerable<AnimalImageModel> animalImageModels)
        {
            var animalImageEntities = animalImageModels.Select(am => _mapper.Map<AnimalImage>(am)).ToList();
            await _dbContext.AddRangeAsync(animalImageEntities);
            await _dbContext.SaveChangesAsync();
            return animalImageEntities;
        }
    }
}
