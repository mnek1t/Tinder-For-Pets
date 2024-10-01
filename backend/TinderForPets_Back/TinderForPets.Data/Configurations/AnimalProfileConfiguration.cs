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
            builder
                 .HasNoKey()
                 .ToTable("animal_profile");

            builder.Property(e => e.Age).HasColumnName("age");
            builder.Property(e => e.Id).HasColumnName("id");
            builder.Property(e => e.Image).HasColumnName("image");
            builder.Property(e => e.Name).HasColumnName("name");
            builder.Property(e => e.SexId).HasColumnName("sex_id");
        }
    }
}
