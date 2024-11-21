using Microsoft.EntityFrameworkCore;
using TinderForPets.Data.Configurations;
using TinderForPets.Data.Entities;

namespace TinderForPets.Data;
public partial class TinderForPetsDbContext : DbContext
{
    public TinderForPetsDbContext(DbContextOptions<TinderForPetsDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Animal> Animals { get; set; }
    public virtual DbSet<AnimalProfile> AnimalProfiles { get; set; }
    public virtual DbSet<AnimalImage> AnimalImage { get; set; }

    public virtual DbSet<AnimalType> AnimalTypes { get; set; }
    public virtual DbSet<Sex> Sexes { get; set; }
    public virtual DbSet<UserAccount> UserAccounts { get; set; }
    public virtual DbSet<Role> Roles { get; set; }
    public virtual DbSet<UserRole> UserRoles { get; set; }
    public virtual DbSet<Breed> Breeds { get; set; }
    public virtual DbSet<Match> Matches { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new AnimalConfiguration());
        modelBuilder.ApplyConfiguration(new AnimalProfileConfiguration());
        modelBuilder.ApplyConfiguration(new AnimalImageConfiguration());
        modelBuilder.ApplyConfiguration(new AnimalTypeConfiguration());
        modelBuilder.ApplyConfiguration(new BreedConfiguration());
        modelBuilder.ApplyConfiguration(new RoleConfiguration());
        modelBuilder.ApplyConfiguration(new SexConfiguration());
        modelBuilder.ApplyConfiguration(new UserAccountConfiguration());
        modelBuilder.ApplyConfiguration(new UserRoleConfiguration());
        modelBuilder.ApplyConfiguration(new MatchConfiguration());

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
