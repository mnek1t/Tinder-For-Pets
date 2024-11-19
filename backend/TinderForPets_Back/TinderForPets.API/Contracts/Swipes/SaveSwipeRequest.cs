using System.ComponentModel.DataAnnotations;

namespace TinderForPets.API.Contracts.Swipes
{
    public record SaveSwipeRequest(
        //[Required] Guid PetSwiperProfileId,
        [Required] Guid PetSwipedOnProfileId,
        [Required] bool IsLike
    );
}
