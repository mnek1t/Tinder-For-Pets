using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TinderForPets.Core.Models;
using TinderForPets.Data.Entities;
using TinderForPets.Data.Exceptions;
using TinderForPets.Data.Interfaces;

namespace TinderForPets.Data.Repositories
{
    public class AnimalProfileRepository : TinderForPetsRepository<AnimalProfile>, IAnimalProfileRepository 
    {

        public AnimalProfileRepository(TinderForPetsDbContext context) : base(context) { }

        public async override Task<Guid> CreateAsync(AnimalProfile animalProfile, CancellationToken cancellationToken)
        {
            await _context.AddAsync(animalProfile, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return animalProfile.Id;
        }

        public override Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<Guid> GetAnimalProfileByOwnerIdAsync(Guid ownerId, CancellationToken cancellationToken)
        {
            var animalProfileId = await _context.Animals
                .Where(a => a.UserId == ownerId)
                .Select(a => a.Profile.Id) 
                .SingleOrDefaultAsync(cancellationToken);

            return animalProfileId;
        }

        public Task<AnimalProfile> GetByAnimalIdAsync(Guid animalId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async override Task<int> UpdateAsync(AnimalProfile animalProfile, CancellationToken cancellationToken)
        {
            var rowsUpdated = await _context.AnimalProfiles
            .Where(a => a.AnimalId == animalProfile.AnimalId)
            .ExecuteUpdateAsync(animal =>
                animal
                .SetProperty(a => a.IsVaccinated, animalProfile.IsVaccinated)
                .SetProperty(a => a.IsSterilized, animalProfile.IsSterilized)
                .SetProperty(a => a.SexId, animalProfile.SexId)
                .SetProperty(a => a.City, animalProfile.City)
                .SetProperty(a => a.Country, animalProfile.Country)
                .SetProperty(a => a.DateOfBirth, animalProfile.DateOfBirth)
                .SetProperty(a => a.Description, animalProfile.Description)
                .SetProperty(a => a.Height, animalProfile.Height)
                .SetProperty(a => a.Weight, animalProfile.Weight)
                .SetProperty(a => a.Latitude, animalProfile.Latitude)
                .SetProperty(a => a.Longitude, animalProfile.Longitude)
                .SetProperty(a => a.Name, animalProfile.Name),
                cancellationToken
            );

            return rowsUpdated == 0 ? throw new AnimalNotFoundException(animalProfile.Id) : rowsUpdated;

        }
    }
}
