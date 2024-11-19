using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TinderForPets.Data.Entities;

namespace TinderForPets.Data.Configurations
{
    public class MatchConfiguration : IEntityTypeConfiguration<Match>
    {
        public void Configure(EntityTypeBuilder<Match> builder)
        {
            builder.HasKey(x => x.Id).HasName("match_pkey");

            builder.HasIndex(m => m.FirstSwiperId).HasDatabaseName("ix_match_first_swiper_id");
            builder.HasIndex(m => m.SecondSwiperId).HasDatabaseName("ix_match_second_swiper_id");


            builder.ToTable("match");
            builder.Property(m => m.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");

            builder.Property(m => m.CreatedAt).HasColumnName("created_at").IsRequired();
            builder.Property(m => m.FirstSwiperId).HasColumnName("first_swiper_id").IsRequired();
            builder.Property(m => m.SecondSwiperId).HasColumnName("second_swiper_id").IsRequired();

            builder.HasOne(m => m.FirstSwiper)
              .WithMany(ap => ap.Matches)
              .HasForeignKey(m => m.FirstSwiperId)
              .OnDelete(DeleteBehavior.Restrict)
              .HasConstraintName("fk_match_first_swiper"); 

            builder.HasOne(m => m.SecondSwiper)
             .WithMany()
             .HasForeignKey(m => m.SecondSwiperId)
             .OnDelete(DeleteBehavior.Restrict)
             .HasConstraintName("fk_match_second_swiper");

        }
    }
}
