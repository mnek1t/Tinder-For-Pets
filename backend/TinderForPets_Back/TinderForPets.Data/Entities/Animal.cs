namespace TinderForPets.Data.Entities;

public class Animal
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public int TypeId { get; set; }
    public int BreedId { get; set; }

    public virtual AnimalType Type { get; set; } = null!;

    public virtual UserAccount User { get; set; } = null!;
    public virtual AnimalProfile Profile { get; set; } = null!;
    public virtual Breed Breed { get; set; } = null!;

}
