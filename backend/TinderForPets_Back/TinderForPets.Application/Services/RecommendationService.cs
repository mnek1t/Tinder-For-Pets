using AutoMapper;
using SharedKernel;
using TinderForPets.Application.DTOs;
using TinderForPets.Application.Interfaces;
using TinderForPets.Core.Models;
using TinderForPets.Data.Entities;
using TinderForPets.Data.Interfaces;

namespace TinderForPets.Application.Services
{
    public class RecommendationService
    {
        private readonly IMapper _mapper;
        private readonly IAnimalProfileRepository _profileRepository;
        private readonly ICacheService _cacheService;
        private readonly SwipeService _swipeService;
        private readonly S2GeometryService _s2GeometryService;
        public RecommendationService(
            IAnimalProfileRepository animalProfileRepository,
            ICacheService cacheService,
            S2GeometryService s2GeometryService,
            SwipeService swipeService,
            IMapper mapper)
        {
            _profileRepository = animalProfileRepository;
            _cacheService = cacheService;
            _s2GeometryService = s2GeometryService;
            _swipeService = swipeService;
            _mapper = mapper;
        }
        public async Task<Result<List<AnimalProfileModel>>> GetRecommendationsForUserAsync(Guid userId, double radiusKm, CancellationToken cancellationToken) 
        {
            // TODO: Cache data
            AnimalProfile animalProfile;
            try
            {
                animalProfile = await _profileRepository.GetAnimalProfileByOwnerIdAsync(userId, cancellationToken);
                var nearbyCells = _s2GeometryService.GetNearbyCellIds(animalProfile.Latitude, animalProfile.Longitude, radiusKm);
                var swipedProfileIdsResult = await _swipeService.GetSwipedProfiles(animalProfile.Id);
                if (swipedProfileIdsResult.IsFailure) 
                {
                    return Result.Failure<List<AnimalProfileModel>>(swipedProfileIdsResult.Error);
                }

                var swipedProfileIds = swipedProfileIdsResult.Value;
                
                var filter = new AnimalRecommendationFilter
                {
                    OppositeSexId = animalProfile.SexId == (int)Gender.Male ? (int)Gender.Female: (int)Gender.Male,
                    NearbyS2CellIds = nearbyCells,
                    SwipedProfileIds = swipedProfileIds,
                    BreedId = animalProfile.Animal.BreedId,
                    AnimalTypeId = animalProfile.Animal.AnimalTypeId
                };

                filter.NearbyS2CellIds.Add(animalProfile.S2CellId); // profile s2 cell is included in search
                var animalProfiles = await _profileRepository
                    .GetAnimalProfilesAsync(
                        AnimalProfileFilterBuilder.BuildAnimalProfileFilter(filter), 
                        cancellationToken);
                var animalProfileModels = animalProfiles.Select(ap => _mapper.Map<AnimalProfileModel>(ap)).ToList();
                return Result.Success<List<AnimalProfileModel>>(animalProfileModels);
            }
            catch (OperationCanceledException)
            {
                return Result.Failure<List<AnimalProfileModel>>(new Error("400", "Operation canceled"));
            }
        }
    }
}
