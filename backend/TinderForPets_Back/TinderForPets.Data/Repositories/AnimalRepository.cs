using Microsoft.EntityFrameworkCore;
using System.Threading;
using TinderForPets.Core.Models;
using TinderForPets.Data.Entities;
using TinderForPets.Data.Exceptions;
using TinderForPets.Data.Interfaces;

namespace TinderForPets.Data.Repositories
{
    public class AnimalRepository(TinderForPetsDbContext context) : TinderForPetsRepository<Animal>(context), IAnimalRepository
    {
        public async override Task<Guid> CreateAsync(Animal animal, CancellationToken cancellationToken)
        {
            await _context.AddAsync(animal, cancellationToken);
            await _context.SaveChangesAsync();
            return animal.Id;
        }
        //public async override Task<Animal> UpdateAsync(Animal animalEntity, CancellationToken cancellationToken)
        //{
        //    var existingAnimal = await _context.Animals
        //        .Include(a => animalEntity.Breed)
        //        .Include(a => animalEntity.Type)
        //        .FirstOrDefaultAsync(a => a.Id == animalEntity.Id && a.UserId == animalEntity.UserId, cancellationToken);

        //    if (existingAnimal == null)
        //    {
        //        throw new AnimalNotFoundException();
        //    }

        //    existingAnimal.AnimalTypeId = animalEntity.AnimalTypeId;
        //    existingAnimal.BreedId = animalEntity.BreedId;
        //    await _context.SaveChangesAsync(cancellationToken);

        //    return existingAnimal;

        //}
        public async override Task<Animal> UpdateAsync(Animal animalEntity, CancellationToken cancellationToken)
        {
            var existingAnimal = await _context.Animals
                .Include(ap => ap.Breed)
                .Include(a => a.Type)
                .FirstOrDefaultAsync(a => a.Id == animalEntity.Id && a.UserId == animalEntity.UserId, cancellationToken);

            if (existingAnimal == null)
                throw new AnimalNotFoundException();

            existingAnimal.AnimalTypeId = animalEntity.AnimalTypeId;
            existingAnimal.BreedId = animalEntity.BreedId;

            await _context.SaveChangesAsync(cancellationToken);
            return existingAnimal;
        }
        public async Task<Animal> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await _dbSet
                .Include(a => a.Profile)
                .Include(a => a.User)
                .FirstOrDefaultAsync(a => a.UserId == userId, cancellationToken);
        }
    }
}
