using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Threading;
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

        public async Task<AnimalProfile> GetAnimalProfileByOwnerIdAsync(Guid ownerId, CancellationToken cancellationToken)
        {
            var animalProfile = await _context.AnimalProfiles
               .Include(ap => ap.Animal) // Load the related Animal entity
               .Where(ap => ap.Animal.UserId == ownerId) // Assuming Animal has a UserId property
               .SingleOrDefaultAsync(cancellationToken);

            return animalProfile;
        }

        public async Task<AnimalProfile> GetAnimalProfileDetails(Guid ownerId, CancellationToken cancellationToken)
        {
            var animalProfile = await _context.AnimalProfiles
                .Include(ap => ap.Animal)
                    .ThenInclude(a => a.Breed)
                    .ThenInclude(a => a.AnimalType)
                .Include(ap => ap.Images)
                .Include(ap => ap.Sex)
                .Where(ap => ap.Animal.UserId == ownerId)
                .SingleOrDefaultAsync(cancellationToken);

            return animalProfile;
        }

        public async Task<List<AnimalProfile>> GetAnimalProfilesAsync(Expression<Func<AnimalProfile, bool>> func, CancellationToken cancellationToken)
        {
            var animalProfiles = await _context.AnimalProfiles
                .Where(func)
                .ToListAsync(cancellationToken);

            return animalProfiles;
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
