namespace TinderForPets.Data.Entities;

public partial class Sex
{
    public Sex(int id, string sexName)
    {
        Id = id;
        SexName = sexName;
    }

    public int Id { get; set; }

    public string SexName { get; set; }

    public ICollection<AnimalProfile> AnimalProfiles { get; set; } = new List<AnimalProfile>();
}
