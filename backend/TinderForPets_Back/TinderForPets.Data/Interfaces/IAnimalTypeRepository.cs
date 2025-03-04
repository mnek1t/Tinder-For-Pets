﻿using System.Threading;
using TinderForPets.Data.Entities;

namespace TinderForPets.Data.Interfaces
{
    public interface IAnimalTypeRepository
    {
        Task<List<AnimalType>> GetAllAnimalTypesAsync(CancellationToken cancellationToken);
    }
}
