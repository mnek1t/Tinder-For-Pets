namespace TinderForPets.Data.Entities
{
    public class Breed
    {
        public Breed(int id, string breedName, int animalTypeId)
        {
            Id = id;
            BreedName = breedName;
            AnimalTypeId = animalTypeId;
        }
        public int Id { get; set; }
        public string BreedName { get; set; } 
        public int AnimalTypeId { get; set; }

        public AnimalType AnimalType { get; set; }

    }
}
