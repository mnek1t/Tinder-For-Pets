namespace TinderForPets.API.Contracts.Profiles
{
    public record UpdateAnimalProfileRequest(
        string Name,
        int TypeId,
        string Description,
        DateOnly DateOfBirth,
        int SexId,
        bool IsVaccinated,
        bool IsSterilized,
        int BreedId,
        string City,
        string Country
        //decimal Height,
        //decimal Weight
        );
}
