using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TinderForPets.Data.Entities;

namespace TinderForPets.Data.Configurations
{
    public class BreedConfiguration : IEntityTypeConfiguration<Breed>
    {
        public void Configure(EntityTypeBuilder<Breed> builder) 
        {
            builder.HasKey(x => x.Id).HasName("breed_pkey");

            builder.ToTable("breed");

            builder.Property(b => b.BreedName)
                .IsRequired()
                .HasColumnName("breed_name");

            builder.Property(b => b.AnimalTypeId)
                .HasColumnName("animal_type_id");

            builder.HasOne(b => b.AnimalType).WithMany(at => at.Breeds)
                 .HasForeignKey(b => b.AnimalTypeId)
                 .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
