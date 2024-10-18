namespace TinderForPets.Data.Entities;

public partial class AnimalProfile
{
    public Guid Id { get; set; }
    public Guid? AnimalId { get; set; }
    public string? Name { get; set; }
    public string Description { get; set; } = string.Empty;

    public int? Age { get; set; }

    public int? SexId { get; set; }

    public bool IsVaccinated { get; set; }
    public bool IsSterilized { get; set; }
    public Animal Animal { get; set; }
    public Sex Sex { get; set;}
    public ICollection<AnimalImage> Images { get; set; } = new List<AnimalImage>();
}
