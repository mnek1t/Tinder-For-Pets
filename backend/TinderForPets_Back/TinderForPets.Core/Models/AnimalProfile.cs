namespace TinderForPets.Core.Models
{
    public class AnimalProfile
    {
        public AnimalProfile()
        {
        }

        public AnimalProfile(Guid id, Guid animalId, string name, string description, int age, int sexId, bool isVaccinated, bool isSterilized, string breed)
        {
            Id = id;
            AnimalId = animalId;
            Name = name;
            Description = description;
            Age = age;
            SexId = sexId;
            IsVaccinated = isVaccinated;
            IsSterilized = isSterilized;
        }

        public Guid Id { get; set; }
        public Guid? AnimalId { get; set; }
        public string? Name { get; private set; }
        public string Description { get; private set; } = string.Empty;

        public int Age { get; private set; }

        public int SexId { get; set; }
        public int BreedId { get; private set; }
        public bool IsVaccinated { get; private set; }
        public bool IsSterilized { get; private set; }
        public static AnimalProfile Create(Guid id, Guid animalId, string name, string description, int age, int sexId, bool isVaccinated, bool isSterilized, string breed) 
        {
            return new AnimalProfile(id, animalId,name,description, age, sexId, isVaccinated, isSterilized, breed);
        }
    }
}
