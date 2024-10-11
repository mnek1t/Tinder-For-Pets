using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TinderForPets.Data.Entities;

namespace TinderForPets.Data.Configurations
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder) 
        {
            builder.HasKey(ur => new { ur.UserId, ur.RoleId });
            
            builder.ToTable("user_role");
            
            builder.Property(ur => ur.RoleId).HasColumnName("role_id");
            builder.Property(ur => ur.UserId).HasColumnName("user_id");


            builder.HasOne(ur => ur.Role).WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId)
                .HasConstraintName("user_role_role_id_fkey");

            builder.HasOne(ur => ur.User).WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId)
                .HasConstraintName("user_role_user_id_fkey");
        }
    }
}
