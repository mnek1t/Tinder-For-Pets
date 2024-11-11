using SharedKernel;
using TinderForPets.Application.Interfaces;

namespace TinderForPets.Application.Services
{
    public class SwipeService
    {
        private readonly ICacheService _cacheService;
        public SwipeService(ICacheService cacheService)
        {
            _cacheService = cacheService;   
        }

        public async Task<Result<List<Guid>>> GetSwipedProfiles(Guid petSwiperProfileId) 
        {
            var swipedProfileIds = await _cacheService.GetByPatternAsync($"RedisCachingswipe:{petSwiperProfileId}:*");
            return Result.Success<List<Guid>>(swipedProfileIds);
        }

        public async Task<Result> SaveSwipeAsync(Guid petSwiperProfileId, Guid petSwipedOnProfielId, bool isLike, CancellationToken cancellationToken) 
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (isLike) 
                {
                    var oppositeSwipeCacheKey = $"swipe:{petSwipedOnProfielId}:{petSwiperProfileId}";
                    var oppositeSwipeKey = await _cacheService.GetAsync<string>(oppositeSwipeCacheKey);
                    if (string.IsNullOrEmpty(oppositeSwipeKey))
                    {
                        //second user has not swiped this profile yet
                    }
                    else if (oppositeSwipeKey == "like")
                    {
                        // it is a match
                    }
                }

                var swipeCacheKey = $"swipe:{petSwiperProfileId}:{petSwipedOnProfielId}";
                await _cacheService.SetAsync(swipeCacheKey, isLike ? "like" : "dislike", TimeSpan.FromHours(24));
                return Result.Success();
            }
            catch (OperationCanceledException)
            {
                return Result.Failure(new Error("400", "Operation is cancelled"));
            }
        }
    }
}
