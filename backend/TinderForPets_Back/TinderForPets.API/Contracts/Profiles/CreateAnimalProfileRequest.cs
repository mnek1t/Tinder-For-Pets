﻿using System.ComponentModel.DataAnnotations;

namespace TinderForPets.API.Contracts.Profiles
{
    public record CreateAnimalProfileRequest(
        [Required] string Name,
        [Required] string Type, // dog or cat
        [Required] string Breed,
            string Description,
        [Required] int Age,
        [Required] string Sex,
        [Required] bool IsVaccinated,
        [Required] bool IsSterilized,
        [Required] Guid OwnerId, // in the top of my head, we need to take this from cookie where jwt token is located. jwt token includes encrypted userId
            byte[] Image
        );
}
