﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TinderForPets.Data;

#nullable disable

namespace TinderForPets.Data.Migrations
{
    [DbContext(typeof(TinderForPetsDbContext))]
    partial class TinderForPetsDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("TinderForPets.Data.Entities.Animal", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<int>("AnimalTypeId")
                        .HasColumnType("integer")
                        .HasColumnName("animal_type_id");

                    b.Property<int>("BreedId")
                        .HasColumnType("integer")
                        .HasColumnName("breed_id");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("animal_pkey");

                    b.HasIndex("AnimalTypeId");

                    b.HasIndex("BreedId");

                    b.HasIndex("UserId");

                    b.ToTable("animal", (string)null);
                });

            modelBuilder.Entity("TinderForPets.Data.Entities.AnimalImage", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("AnimalProfileId")
                        .HasColumnType("uuid")
                        .HasColumnName("animal_profile_id");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<byte[]>("ImageData")
                        .IsRequired()
                        .HasColumnType("bytea")
                        .HasColumnName("image_data");

                    b.Property<string>("ImageFormat")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("image_format");

                    b.Property<DateOnly>("UploadDate")
                        .HasColumnType("date");

                    b.HasKey("Id")
                        .HasName("aminal_image_pkey");

                    b.HasIndex("AnimalProfileId");

                    b.ToTable("animal_image", (string)null);
                });

            modelBuilder.Entity("TinderForPets.Data.Entities.AnimalProfile", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<decimal>("Age")
                        .HasColumnType("numeric(2,0)")
                        .HasColumnName("age");

                    b.Property<Guid?>("AnimalId")
                        .IsRequired()
                        .HasColumnType("uuid")
                        .HasColumnName("animal_id");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("city");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("country");

                    b.Property<DateOnly>("DateOfBirth")
                        .HasColumnType("date")
                        .HasColumnName("date_of_birth");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<decimal>("Height")
                        .HasColumnType("numeric")
                        .HasColumnName("height");

                    b.Property<bool>("IsSterilized")
                        .HasColumnType("boolean")
                        .HasColumnName("is_sterilized");

                    b.Property<bool>("IsVaccinated")
                        .HasColumnType("boolean")
                        .HasColumnName("is_vaccinated");

                    b.Property<double>("Latitude")
                        .HasColumnType("double precision")
                        .HasColumnName("latitude");

                    b.Property<double>("Longitude")
                        .HasColumnType("double precision")
                        .HasColumnName("longitude");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<long>("S2CellId")
                        .HasColumnType("bigint")
                        .HasColumnName("s2_cell_id");

                    b.Property<int>("SexId")
                        .HasColumnType("integer")
                        .HasColumnName("sex_id");

                    b.Property<decimal>("Weight")
                        .HasColumnType("numeric")
                        .HasColumnName("weight");

                    b.HasKey("Id")
                        .HasName("animal_profile_pkey");

                    b.HasIndex("AnimalId")
                        .IsUnique();

                    b.HasIndex("S2CellId")
                        .HasDatabaseName("ix_s2_cell_id");

                    b.HasIndex("SexId");

                    b.ToTable("animal_profile", null, t =>
                        {
                            t.HasCheckConstraint("chk_age_ge_than_1", "age >= 1");

                            t.HasCheckConstraint("chk_height_ge_than_10", "height IS NULL OR height >= 10");

                            t.HasCheckConstraint("chk_weight_ge_than_0_3", "weight IS NULL OR weight >= 0.3");
                        });
                });

            modelBuilder.Entity("TinderForPets.Data.Entities.AnimalType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("TypeName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("type_name");

                    b.HasKey("Id")
                        .HasName("animal_type_pkey");

                    b.HasIndex(new[] { "TypeName" }, "animal_type_type_name_key")
                        .IsUnique();

                    b.ToTable("animal_type", (string)null);
                });

            modelBuilder.Entity("TinderForPets.Data.Entities.Breed", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("AnimalTypeId")
                        .HasColumnType("integer")
                        .HasColumnName("animal_type_id");

                    b.Property<string>("BreedName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("breed_name");

                    b.HasKey("Id")
                        .HasName("breed_pkey");

                    b.HasIndex("AnimalTypeId");

                    b.ToTable("breed", (string)null);
                });

            modelBuilder.Entity("TinderForPets.Data.Entities.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("role_name");

                    b.HasKey("Id");

                    b.ToTable("role", (string)null);
                });

            modelBuilder.Entity("TinderForPets.Data.Entities.Sex", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("SexName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("sex_name");

                    b.HasKey("Id")
                        .HasName("sex_pkey");

                    b.ToTable("sex", (string)null);
                });

            modelBuilder.Entity("TinderForPets.Data.Entities.UserAccount", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("EmailAddress")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("email_address");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean")
                        .HasColumnName("email_confirmed");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("password");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("user_name");

                    b.HasKey("Id")
                        .HasName("user_account_pkey");

                    b.ToTable("user_account", (string)null);
                });

            modelBuilder.Entity("TinderForPets.Data.Entities.UserRole", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.Property<int>("RoleId")
                        .HasColumnType("integer")
                        .HasColumnName("role_id");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("user_role", (string)null);
                });

            modelBuilder.Entity("TinderForPets.Data.Entities.Animal", b =>
                {
                    b.HasOne("TinderForPets.Data.Entities.AnimalType", "Type")
                        .WithMany("Animals")
                        .HasForeignKey("AnimalTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("animal_type_id_fkey");

                    b.HasOne("TinderForPets.Data.Entities.Breed", "Breed")
                        .WithMany()
                        .HasForeignKey("BreedId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("animal_breed_id_fkey");

                    b.HasOne("TinderForPets.Data.Entities.UserAccount", "User")
                        .WithMany("Animals")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("animal_user_id_fkey");

                    b.Navigation("Breed");

                    b.Navigation("Type");

                    b.Navigation("User");
                });

            modelBuilder.Entity("TinderForPets.Data.Entities.AnimalImage", b =>
                {
                    b.HasOne("TinderForPets.Data.Entities.AnimalProfile", "AnimalProfile")
                        .WithMany("Images")
                        .HasForeignKey("AnimalProfileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("animal_image_id_fkey");

                    b.Navigation("AnimalProfile");
                });

            modelBuilder.Entity("TinderForPets.Data.Entities.AnimalProfile", b =>
                {
                    b.HasOne("TinderForPets.Data.Entities.Animal", "Animal")
                        .WithOne("Profile")
                        .HasForeignKey("TinderForPets.Data.Entities.AnimalProfile", "AnimalId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("animal_profile_id_fkey");

                    b.HasOne("TinderForPets.Data.Entities.Sex", "Sex")
                        .WithMany("AnimalProfiles")
                        .HasForeignKey("SexId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("animal_profile_sex_id_fkey");

                    b.Navigation("Animal");

                    b.Navigation("Sex");
                });

            modelBuilder.Entity("TinderForPets.Data.Entities.Breed", b =>
                {
                    b.HasOne("TinderForPets.Data.Entities.AnimalType", "AnimalType")
                        .WithMany("Breeds")
                        .HasForeignKey("AnimalTypeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("animal_breed_type_id_fkey");

                    b.Navigation("AnimalType");
                });

            modelBuilder.Entity("TinderForPets.Data.Entities.UserRole", b =>
                {
                    b.HasOne("TinderForPets.Data.Entities.Role", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("user_role_role_id_fkey");

                    b.HasOne("TinderForPets.Data.Entities.UserAccount", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("user_role_user_id_fkey");

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("TinderForPets.Data.Entities.Animal", b =>
                {
                    b.Navigation("Profile")
                        .IsRequired();
                });

            modelBuilder.Entity("TinderForPets.Data.Entities.AnimalProfile", b =>
                {
                    b.Navigation("Images");
                });

            modelBuilder.Entity("TinderForPets.Data.Entities.AnimalType", b =>
                {
                    b.Navigation("Animals");

                    b.Navigation("Breeds");
                });

            modelBuilder.Entity("TinderForPets.Data.Entities.Role", b =>
                {
                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("TinderForPets.Data.Entities.Sex", b =>
                {
                    b.Navigation("AnimalProfiles");
                });

            modelBuilder.Entity("TinderForPets.Data.Entities.UserAccount", b =>
                {
                    b.Navigation("Animals");

                    b.Navigation("UserRoles");
                });
#pragma warning restore 612, 618
        }
    }
}
