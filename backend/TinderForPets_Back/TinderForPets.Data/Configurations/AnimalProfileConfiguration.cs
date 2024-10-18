using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TinderForPets.Data.Entities;

namespace TinderForPets.Data.Configurations
{
    public class AnimalProfileConfiguration : IEntityTypeConfiguration<AnimalProfile>
    {
        // TODO: make configurations
        public void Configure(EntityTypeBuilder<AnimalProfile> builder)
        {
            builder.HasKey(e => e.Id).HasName("animal_profile_pkey");

            builder.ToTable("animal_profile").HasCheckConstraint("chk_age_ge_than_1", "age >= 1");

            builder.Property(e => e.Id).IsRequired().HasColumnName("id");
            builder.Property(e => e.AnimalId).IsRequired().HasColumnName("animal_id");
            builder.Property(e => e.Name).IsRequired().HasColumnName("name");
            builder.Property(e => e.Description).HasColumnName("description");
            builder.Property(e => e.Age).IsRequired().HasColumnName("age").HasColumnType("numeric(2,0)");
            builder.Property(e => e.SexId).IsRequired().HasColumnName("sex_id");
            builder.Property(e => e.IsVaccinated).IsRequired().HasColumnName("is_vaccinated");
            builder.Property(e => e.IsSterilized).IsRequired().HasColumnName("is_sterilized");

            builder.HasMany(ap => ap.Images).WithOne(i => i.AnimalProfile)
                .HasForeignKey(i => i.AnimalProfileId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("animal_image_id_fkey");

            builder.HasOne(ap => ap.Sex).WithMany(s => s.AnimalProfiles)
                .HasForeignKey(ap => ap.SexId)
                .HasConstraintName("animal_profile_sex_id_fkey");
        }
    }
}
