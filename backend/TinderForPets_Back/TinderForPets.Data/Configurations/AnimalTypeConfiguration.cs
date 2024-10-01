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
    public class AnimalTypeConfiguration : IEntityTypeConfiguration<AnimalType>
    {
        // TODO: make configurations
        public void Configure(EntityTypeBuilder<AnimalType> builder)
        {
            builder.HasKey(e => e.Id).HasName("animal_type_pkey");

            builder.ToTable("animal_type");

            builder.HasIndex(e => e.TypeName, "animal_type_type_name_key").IsUnique();

            builder.Property(e => e.Id).HasColumnName("id");
            builder.Property(e => e.TypeName).HasColumnName("type_name");
        }
    }
}
