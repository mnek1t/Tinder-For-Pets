using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinderForPets.Data.Entities;

namespace TinderForPets.Data.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder) 
        {
            builder.HasKey(x => x.Id);

            builder.ToTable("role");

            builder.Property(r => r.Id).HasColumnName("id");
            builder.Property(r => r.RoleName).HasColumnName("role_name");
        }
    }
}
