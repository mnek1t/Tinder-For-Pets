using AutoMapper;
using SharedKernel;
using TinderForPets.Core;
using TinderForPets.Core.Models;
using TinderForPets.Data.Entities;
using TinderForPets.Data.Interfaces;

namespace TinderForPets.Data.Repositories
{
    public class AnimalProfileRepository : IAnimalProfileRepository
    {
        private readonly TinderForPetsDbContext _context;
        private readonly IMapper _mapper;

        public AnimalProfileRepository(TinderForPetsDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Guid> CreateAnimalAsync(AnimalModel animalModel) 
        {
            if (animalModel == null)
            {
                return Guid.Empty;
            }

            var animalEntity = _mapper.Map<Animal>(animalModel);
            await _context.AddAsync(animalEntity);
            return animalEntity.Id;
        }

        public async Task<Guid> CreateProfileAsync(AnimalProfileModel animalProfile)
        {
            if (animalProfile == null) 
            {
                return Guid.Empty;
            }

            var animalProfileEntity = _mapper.Map<AnimalProfile>(animalProfile);
            await _context.AddAsync(animalProfileEntity);
            await _context.SaveChangesAsync();
            return animalProfileEntity.Id;
        }

        public void DeleteProfile()
        {
            throw new NotImplementedException();
        }

        public void UpdateProfile()
        {
            throw new NotImplementedException();
        }
    }
}
