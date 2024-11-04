namespace TinderForPets.Data.Entities;

public partial class AnimalType
{
    public AnimalType(int id, string typeName)
    {
        Id = id;
        TypeName = typeName;
    }

    public int Id { get; set; }

    public string TypeName { get; set; }

    public virtual ICollection<Animal> Animals { get; set; } = new List<Animal>();
    public ICollection<Breed> Breeds { get; set; } = new List<Breed>();
}
