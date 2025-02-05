﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TinderForPets.Data.Entities;

namespace TinderForPets.Data.Configurations
{
    public class AnimalConfiguration : IEntityTypeConfiguration<Animal>
    {
        // TODO: make configurations
        public void Configure(EntityTypeBuilder<Animal> builder)
        {
            builder.HasKey(e => e.Id).HasName("animal_pkey");

            builder.ToTable("animal");

            builder.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            builder.Property(e => e.AnimalTypeId).HasColumnName("animal_type_id");
            builder.Property(e => e.UserId).HasColumnName("user_id");
            builder.Property(e => e.BreedId).HasColumnName("breed_id");

            builder.HasOne(d => d.Type).WithMany(p => p.Animals)
                .HasForeignKey(d => d.AnimalTypeId)
                //.OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("animal_type_id_fkey");

            builder.HasOne(d => d.User).WithMany(p => p.Animals)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("animal_user_id_fkey");

            //TODO WHy has one?
            builder.HasOne(a => a.Profile).WithOne(ap => ap.Animal)
                .HasForeignKey<AnimalProfile>(p => p.AnimalId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("animal_profile_id_fkey");

            builder.HasOne(a => a.Breed).WithMany()
                .HasForeignKey(a => a.BreedId)
                .HasConstraintName("animal_breed_id_fkey");

        }
    }
}
