namespace TinderForPets.Data.Entities
{
    public class Breed
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int AnimalTypeId { get; set; }

        public AnimalType AnimalType { get; set; }

    }
}
