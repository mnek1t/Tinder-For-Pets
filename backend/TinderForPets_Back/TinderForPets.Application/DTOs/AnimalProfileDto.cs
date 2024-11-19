namespace TinderForPets.Application.DTOs
{
    public class AnimalProfileDto
    {
        public Guid Id { get; set; }
        public Guid AnimalId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Age { get; set; }
        public DateOnly DateOfBirth { get; set; }

        public int SexId { get; set; }
        public string Sex { get; set; } = string.Empty;

        public bool IsVaccinated { get; set; }
        public bool IsSterilized { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public ulong S2CellId { get; set; }
        public string Country { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public decimal Height { get; set; }
        public decimal Weight { get; set; }
    }
}
