using SharedKernel;
namespace TinderForPets.Core
{
    public static class AnimalProfileErrors
    {
        public static readonly Error NotCreatedProfile = new(
        "AnimalProfile.NotCreated", "The provided pet profile was not created (null)");

        public static readonly Error NotCreatedAnimal = new(
        "Animal.NotCreated", "The provided pet was not created (null)");

        public static readonly Error NotFound = new(
        "Animal.NotFoundById", "The provided pet was not found");

        public static readonly Error ImageIsNotFound = new(
       "Animal.ImageIsNotFound", "The iamge of animal profile was not found");

        public static Error NotUpdated(string message) => new(
        "Animal.NotUpdated", message);

        public static Error BreedNotFound(int id) => new(
        "Breed.NotFoundById", $"Breed were not found for this animal type id: {id}");
    }
}
