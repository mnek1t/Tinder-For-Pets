using TinderForPets.Core.Models;

namespace TinderForPets.Application.DTOs
{
    public class AnimalRecommendationFilter
    {
        public int? OppositeSexId { get; set; }
        public List<ulong> NearbyS2CellIds { get; set; } = new List<ulong>();
        public int? BreedId { get; set; }
        public int? AnimalTypeId { get; set; } 
    }
}
