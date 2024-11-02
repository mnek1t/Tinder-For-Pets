using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TinderForPets.Core.Models;
using TinderForPets.Data.Entities;
using TinderForPets.Data.Exceptions;
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

        public async Task<int> UpdateAnimalAsync(AnimalModel animalModel)
        {
            if (animalModel == null) 
            {
                throw new AnimalNotFoundException();
            }

            var rowsUpdated = await _context.Animals
            .Where(a => a.Id == animalModel.Id && a.UserId == animalModel.UserId)
            .ExecuteUpdateAsync(animal =>
                animal
                .SetProperty(a => a.TypeId, animalModel.TypeId)
                .SetProperty(a => a.BreedId, animalModel.BreedId)
            );

            return rowsUpdated == 0 ? throw new AnimalNotFoundException(animalModel.Id) : rowsUpdated;
            
        }

        public async Task<int> UpdateProfileAsync(AnimalProfileModel animalProfileModel)
        {
            if (animalProfileModel == null)
            {
                throw new AnimalNotFoundException();
            }

            var rowsUpdated = await _context.AnimalProfiles
            .Where(a => a.AnimalId == animalProfileModel.AnimalId)
            .ExecuteUpdateAsync(animal =>
                animal
                .SetProperty(a => a.IsVaccinated, animalProfileModel.IsVaccinated)
                .SetProperty(a => a.IsSterilized, animalProfileModel.IsSterilized)
                .SetProperty(a => a.SexId, animalProfileModel.SexId)
                .SetProperty(a => a.City, animalProfileModel.City)
                .SetProperty(a => a.Country, animalProfileModel.Country)
                .SetProperty(a => a.DateOfBirth, animalProfileModel.DateOfBirth)
                .SetProperty(a => a.Description, animalProfileModel.Description)
                .SetProperty(a => a.Height, animalProfileModel.Height)
                .SetProperty(a => a.Weight, animalProfileModel.Weight)
                .SetProperty(a => a.Latitude, animalProfileModel.Latitude)
                .SetProperty(a => a.Longitude, animalProfileModel.Longitude)
                .SetProperty(a => a.Name, animalProfileModel.Name)
            );

            return rowsUpdated == 0 ? throw new AnimalNotFoundException(animalProfileModel.Id) : rowsUpdated;

        }
    }
}
