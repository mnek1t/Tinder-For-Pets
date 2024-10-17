using SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinderForPets.Data.Entities;

namespace TinderForPets.Data.Interfaces
{
    public interface IAnimalTypeRepository
    {
        Task<Result<List<AnimalType>>> GetAllAnimalTypesAsync();
    }
}
