namespace TinderForPets.Core.Models
{
    public class AnimalProfileModel
    {
        public AnimalProfileModel()
        {
        }

        public AnimalProfileModel(
            Guid id, 
            Guid animalId, 
            string name, 
            string description,
            DateOnly dateOfBirth,
            int sexId, 
            bool isVaccinated, 
            bool isSterilized, 
            string country, 
            string city, 
            decimal latitude, 
            decimal longitude,
            decimal height,
            decimal weight
            )
        {
            Id = id;
            AnimalId = animalId;
            Name = name;
            Description = description;
            DateOfBirth = dateOfBirth;
            SexId = sexId;
            IsVaccinated = isVaccinated;
            IsSterilized = isSterilized;
            Country = country;
            City = city;
            Latitude = latitude;
            Longitude = longitude;
            Height = height;
            Weight = weight;  
        }

        public Guid Id { get; set; }
        public Guid AnimalId { get; set; }
        public string? Name { get; private set; }
        public string Description { get; private set; } = string.Empty;

        public int Age { get; private set; }

        public int SexId { get; set; }
        public bool IsVaccinated { get; private set; }
        public bool IsSterilized { get; private set; }
        public string Country { get; set; }
        public string City { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public decimal Height { get; set; }
        public decimal Weight { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public static AnimalProfileModel Create(
            Guid id, 
            Guid animalId, 
            string name, 
            string description,
            DateOnly dateOfBirth, 
            int sexId, 
            bool isVaccinated, 
            bool isSterilized, 
            string country, 
            string city, 
            decimal latitude, 
            decimal longitude,
            decimal height,
            decimal weight) 
        {
            return new AnimalProfileModel(
                id, 
                animalId,
                name,
                description, 
                dateOfBirth, 
                sexId, 
                isVaccinated, 
                isSterilized, 
                country, 
                city, 
                latitude, 
                longitude,
                height,
                weight
            );
        }
    }
}
