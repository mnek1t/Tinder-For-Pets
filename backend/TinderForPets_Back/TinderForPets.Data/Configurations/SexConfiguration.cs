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
    public class SexConfiguration : IEntityTypeConfiguration<Sex>
    {
        // TODO: make configurations
        public void Configure(EntityTypeBuilder<Sex> builder)
        {
            builder.HasKey(e => e.Id).HasName("sex_pkey");

            builder.ToTable("sex");

            builder.Property(e => e.Id).HasColumnName("id");
            builder.Property(e => e.SexName).HasColumnName("sex_name");
        }
    }
}
