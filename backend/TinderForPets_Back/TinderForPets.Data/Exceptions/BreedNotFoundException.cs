using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinderForPets.Data.Exceptions
{
    public class BreedNotFoundException : Exception
    {
        public BreedNotFoundException() : base("Breeds not found according to this Amimal Type Id") { }
    }
}
