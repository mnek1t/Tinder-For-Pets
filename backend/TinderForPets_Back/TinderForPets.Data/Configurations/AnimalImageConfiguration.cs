using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TinderForPets.Data.Entities;

namespace TinderForPets.Data.Configurations
{
    public class AnimalImageConfiguration : IEntityTypeConfiguration<AnimalImage>
    {
        public void Configure(EntityTypeBuilder<AnimalImage> builder)
        {
            builder.HasKey(ai => ai.Id).HasName("aminal_image_pkey");
            
            builder.ToTable("animal_image");

            builder.Property(ai => ai.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            builder.Property(ai => ai.AnimalProfileId)
                .HasColumnName("animal_profile_id");
            builder.Property(ai => ai.ImageData)
                .HasColumnName("image_data");
            builder.Property(ai => ai.Description)
                .HasColumnName("description");
            builder.Property(ai => ai.ImageFormat)
                .HasColumnName("image_format");
        }
    }
}
