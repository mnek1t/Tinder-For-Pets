using SharedKernel;
using TinderForPets.Application.Interfaces;
using TinderForPets.Core;
using TinderForPets.Data.Entities;
using TinderForPets.Data.Exceptions;
using TinderForPets.Data.Interfaces;

namespace TinderForPets.Application.Services
{
    public class SwipeService
    {
        private readonly ICacheService _cacheService;
        private readonly IAnimalProfileRepository _animalProfileRepository;
        private readonly IMatchRepository _matchRepository;

        public SwipeService(
            ICacheService cacheService, 
            IAnimalProfileRepository animalProfileRepository,
            IMatchRepository matchRepository)
        {
            _cacheService = cacheService;
            _animalProfileRepository = animalProfileRepository;
            _matchRepository = matchRepository;
        }

        public async Task<Result<List<Guid>>> GetSwipedProfiles(Guid petSwiperProfileId) 
        {
            var swipedProfileIds = await _cacheService.GetByPatternAsync($"RedisCachingswipe:{petSwiperProfileId}:*");
            return Result.Success<List<Guid>>(swipedProfileIds);
        }

        public async Task<Result> SaveSwipeAsync(Guid userId, Guid petSwipedOnProfielId, bool isLike, CancellationToken cancellationToken) 
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                var cacheKey = $"animal_details_of_owner{userId}";
                var animalData = await _cacheService.GetAsync<AnimalProfile>(cacheKey);
                if (animalData == null)
                {
                    animalData = await _animalProfileRepository.GetAnimalProfileDetails(userId, cancellationToken);
                    // TODO: If I save all details, they can be updated, so I need to update the cache then
                    await _cacheService.SetAsync<AnimalProfile>(cacheKey, animalData, TimeSpan.FromHours(24));
                }

                if (isLike) 
                {
                    var oppositeSwipeCacheKey = $"swipe:{petSwipedOnProfielId}:{animalData.Id}";
                    var oppositeSwipeKey = await _cacheService.GetAsync<string>(oppositeSwipeCacheKey);
                    if (string.IsNullOrEmpty(oppositeSwipeKey))
                    {
                        //second user has not swiped this profile yet
                    }
                    else if (oppositeSwipeKey == "like")
                    {
                        var match = new Match()
                        {
                            Id = Guid.NewGuid(),
                            CreatedAt = DateTime.UtcNow,
                            FirstSwiperId = petSwipedOnProfielId,
                            SecondSwiperId = animalData.Id
                        };
                        var matchId = await _matchRepository.CreateAsync(match, cancellationToken);
                        // TODO: Notification center queue WebSocket
                    }
                }

                var swipeCacheKey = $"swipe:{animalData.Id}:{petSwipedOnProfielId}";
                await _cacheService.SetAsync(swipeCacheKey, isLike ? "like" : "dislike", TimeSpan.FromHours(24));
                return Result.Success();
            }
            catch (OperationCanceledException)
            {
                return Result.Failure(new Error("400", "Operation is cancelled"));
            }
            catch (AnimalNotFoundException)
            {
                return Result.Failure(AnimalProfileErrors.NotFound);
            }
        }
    }
}
