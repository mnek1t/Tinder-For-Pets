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
               .Include(ap => ap.Animal)
               .Where(ap => ap.Animal.UserId == ownerId)
               .SingleOrDefaultAsync(cancellationToken);

            if (animalProfile == null)
            {
                throw new AnimalNotFoundException();
            }
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

            if (animalProfile == null)
            {
                throw new AnimalNotFoundException();
            }

            return animalProfile;
        }

        public async Task<List<AnimalProfile>> GetAnimalProfilesAsync(Expression<Func<AnimalProfile, bool>> func, CancellationToken cancellationToken)
        {
            var animalProfiles = await _context.AnimalProfiles
                .Include(ap => ap.Images)
                .Where(func)
                .Where(ap => ap.Images != null && ap.Images.Any())
                .ToListAsync(cancellationToken);

            return animalProfiles;
        }

        public async Task<List<AnimalProfile>> GetAnimalProfilesFromIdListAsync(List<Guid> profileIds, CancellationToken cancellationToken)
        {
            var animalProfiles = await _context.AnimalProfiles
                .Include(ap => ap.Images)
                .Where(ap => profileIds.Contains(ap.Id))
                .Where(ap => ap.Images != null && ap.Images.Any())
                .ToListAsync(cancellationToken);

            return animalProfiles;
        }

        public Task<AnimalProfile> GetByAnimalIdAsync(Guid animalId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async override Task<AnimalProfile> UpdateAsync(AnimalProfile animalProfile, CancellationToken cancellationToken)
        {
            var existingAnimalProfile = await _context.AnimalProfiles.Include(ap => ap.Sex).FirstOrDefaultAsync(ap => ap.AnimalId == animalProfile.AnimalId, cancellationToken);

            if (existingAnimalProfile == null)
                throw new AnimalNotFoundException();

            existingAnimalProfile.IsVaccinated = animalProfile.IsVaccinated;
            existingAnimalProfile.IsSterilized = animalProfile.IsSterilized;
            existingAnimalProfile.SexId = animalProfile.SexId;
            existingAnimalProfile.City = animalProfile.City;
            existingAnimalProfile.Country = animalProfile.Country;
            existingAnimalProfile.DateOfBirth = animalProfile.DateOfBirth;
            existingAnimalProfile.Description = animalProfile.Description;
            existingAnimalProfile.Height = animalProfile.Height;
            existingAnimalProfile.Weight = animalProfile.Weight;
            existingAnimalProfile.Latitude = animalProfile.Latitude;
            existingAnimalProfile.Longitude = animalProfile.Longitude;
            existingAnimalProfile.Name = animalProfile.Name;

            await _context.SaveChangesAsync(cancellationToken); 
            return existingAnimalProfile;

        }
    }
}
