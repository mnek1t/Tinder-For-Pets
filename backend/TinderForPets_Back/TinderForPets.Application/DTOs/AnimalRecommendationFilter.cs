using TinderForPets.Core.Models;

namespace TinderForPets.Application.DTOs
{
    public class AnimalRecommendationFilter
    {
        public int? OppositeSexId { get; set; }
        public List<ulong> NearbyS2CellIds { get; set; } = new List<ulong>();
        public List<Guid> SwipedProfileIds { get; set; } = new List<Guid>();

        public int? BreedId { get; set; }
        public int? AnimalTypeId { get; set; }
        public List<Guid> MatchesIds { get; set; } = new List<Guid>();
    }
}
