using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TinderForPets.Data.Entities;

namespace TinderForPets.Data.Configurations
{
    public class UserAccountConfiguration : IEntityTypeConfiguration<UserAccount>
    {
        // TODO: make configurations
        public void Configure(EntityTypeBuilder<UserAccount> builder)
        {
            builder.HasKey(e => e.Id).HasName("user_account_pkey");

            builder.ToTable("user_account");

            builder.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            builder.Property(e => e.EmailAddress).HasColumnName("email_address");
            builder.Property(e => e.Password).HasColumnName("password");
            builder.Property(e => e.UserName).HasColumnName("user_name");
        }
    }
}
