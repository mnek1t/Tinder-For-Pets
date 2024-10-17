using AutoMapper;
using SharedKernel;
using TinderForPets.Core;
using TinderForPets.Core.Models;
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


        public async Task<Result<Guid>> CreateProfile(AnimalProfile animalProfile)
        {
            if (animalProfile == null) 
            {
                return Result.Failure<Guid>(AnimalProfileErrors.NotCreated);
            }

            var animalProfileEntity = _mapper.Map<AnimalProfile>(animalProfile);
            throw new NotImplementedException();
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
