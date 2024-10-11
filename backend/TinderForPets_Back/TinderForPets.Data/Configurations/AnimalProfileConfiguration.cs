using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinderForPets.Data.Entities;

namespace TinderForPets.Data.Configurations
{
    public class AnimalProfileConfiguration : IEntityTypeConfiguration<AnimalProfile>
    {
        // TODO: make configurations
        public void Configure(EntityTypeBuilder<AnimalProfile> builder)
        {
            builder.HasKey(e => e.Id).HasName("animal_profile_pkey");
            
            builder.ToTable("animal_profile");

            builder.Property(e => e.Id).HasColumnName("id");
            builder.Property(e => e.AnimalId).HasColumnName("animal_id");
            builder.Property(e => e.Name).HasColumnName("name");
            builder.Property(e => e.Description).HasColumnName("description");
            builder.Property(e => e.Age).HasColumnName("age");
            builder.Property(e => e.SexId).HasColumnName("sex_id");
            builder.Property(e => e.IsVaccinated).HasColumnName("is_vaccinated");
            builder.Property(e => e.IsSterilized).HasColumnName("is_sterilized");

            builder.HasMany(ap => ap.Images).WithOne(i => i.AnimalProfile)
                .HasForeignKey(i => i.AnimalProfileId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("animal_image_id_fkey");
        }
    }
}
