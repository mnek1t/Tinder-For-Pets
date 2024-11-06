namespace TinderForPets.Data.Entities;

public partial class UserAccount : IEntity
{
    public UserAccount()
    {
    }
    public Guid Id { get; set; }

    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string EmailAddress { get; set; } = null!;
    public bool EmailConfirmed { get; set; } = false;
    public virtual ICollection<Animal> Animals { get; set; } = new List<Animal>();
    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
