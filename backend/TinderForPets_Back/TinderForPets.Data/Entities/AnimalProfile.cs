namespace TinderForPets.Data.Entities;

public partial class AnimalProfile : IEntity
{
    public Guid Id { get; set; }
    public Guid? AnimalId { get; set; }
    public string? Name { get; set; }
    public string Description { get; set; } = string.Empty;

    public int Age { get; set; }
    public DateOnly DateOfBirth { get; set; }

    public int SexId { get; set; }

    public bool IsVaccinated { get; set; }
    public bool IsSterilized { get; set; }

    public decimal Height { get; set; }
    public decimal Weight { get; set; }
    public Animal Animal { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }

    public ulong S2CellId { get; set; }
    public Sex Sex { get; set;}
    public ICollection<AnimalImage> Images { get; set; } = new List<AnimalImage>();
}
