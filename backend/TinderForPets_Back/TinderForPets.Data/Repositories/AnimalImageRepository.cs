
using Microsoft.EntityFrameworkCore;
using TinderForPets.Data.Entities;
using TinderForPets.Data.Interfaces;

namespace TinderForPets.Data.Repositories
{
    public class AnimalImageRepository : TinderForPetsRepository<AnimalImage>, IAnimalImageRepository
    {
        public AnimalImageRepository(TinderForPetsDbContext context) : base(context) { }

        public async Task<AnimalImage> GetAnimalImageAsync(Guid animalProfileId, CancellationToken cancellationToken)
        {
            var image = await _context.AnimalImage
                .Where(ai => ai.AnimalProfileId == animalProfileId)
                .Select(a => new AnimalImage() { ImageData = a.ImageData, ImageFormat = a.ImageFormat})
                .FirstOrDefaultAsync(cancellationToken);

            return image;
        }

        public async override Task<Guid> CreateAsync(AnimalImage animalImage, CancellationToken cancellationToken)
        {
            await _context.AddAsync(animalImage, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return animalImage.Id;
        }

        public override Task UpdateAsync(AnimalImage entity, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
