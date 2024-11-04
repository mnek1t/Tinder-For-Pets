using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TinderForPets.Core.Models;
using TinderForPets.Data.Entities;
using TinderForPets.Data.Interfaces;

namespace TinderForPets.Data.Repositories
{
    public class AnimalImageRepository : IAnimalImageRepository
    {
        private readonly TinderForPetsDbContext _context;
        private readonly IMapper _mapper;
        public AnimalImageRepository(TinderForPetsDbContext dbContext, IMapper mapper)
        {
            _context = dbContext;
            _mapper = mapper;
        }

        public async Task<AnimalImage> GetAnimalImageAsync(Guid animalProfileId)
        {
            var image = await _context.AnimalImage
                .Where(ai => ai.AnimalProfileId == animalProfileId)
                .Select(a => new AnimalImage() { ImageData = a.ImageData, ImageFormat = a.ImageFormat})
                .FirstOrDefaultAsync();

            return image;
        }

        public async Task<List<AnimalImage>> SaveAnimalMediaAsync(IEnumerable<AnimalImageModel> animalImageModels)
        {
            var animalImageEntities = animalImageModels.Select(am => _mapper.Map<AnimalImage>(am)).ToList();
            await _context.AddRangeAsync(animalImageEntities);
            await _context.SaveChangesAsync();
            return animalImageEntities;
        }
    }
}
