namespace TinderForPets.Application.DTOs
{
    public class AnimalProfileDto
    {
        public Guid AnimalId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Age { get; set; }
        public int SexId { get; set; }
        public bool IsVaccinated { get; set; }
        public bool IsSterilized { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string Country { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public decimal Height { get; set; }
        public decimal Width { get; set; }
    }
}
