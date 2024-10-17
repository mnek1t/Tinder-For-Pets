using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TinderForPets.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAnimalProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "animal_type",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    type_name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("animal_type_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "role",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    role_name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_role", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sex",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sex_name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("sex_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user_account",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_name = table.Column<string>(type: "text", nullable: false),
                    password = table.Column<string>(type: "text", nullable: false),
                    email_address = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("user_account_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "animal",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    type_id = table.Column<int>(type: "integer", nullable: false),
                    age = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("animal_pkey", x => x.id);
                    table.ForeignKey(
                        name: "animal_type_id_fkey",
                        column: x => x.type_id,
                        principalTable: "animal_type",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "animal_user_id_fkey",
                        column: x => x.user_id,
                        principalTable: "user_account",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "user_role",
                columns: table => new
                {
                    role_id = table.Column<int>(type: "integer", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_role", x => new { x.user_id, x.role_id });
                    table.ForeignKey(
                        name: "user_role_role_id_fkey",
                        column: x => x.role_id,
                        principalTable: "role",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "user_role_user_id_fkey",
                        column: x => x.user_id,
                        principalTable: "user_account",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "animal_profile",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    animal_id = table.Column<Guid>(type: "uuid", nullable: true),
                    name = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: false),
                    age = table.Column<int>(type: "integer", nullable: true),
                    sex_id = table.Column<int>(type: "integer", nullable: true),
                    is_vaccinated = table.Column<bool>(type: "boolean", nullable: false),
                    is_sterilized = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("animal_profile_pkey", x => x.id);
                    table.ForeignKey(
                        name: "animal_profile_id_fkey",
                        column: x => x.animal_id,
                        principalTable: "animal",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "animal_image",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    animal_profile_id = table.Column<Guid>(type: "uuid", nullable: false),
                    image_data = table.Column<byte[]>(type: "bytea", nullable: false),
                    image_format = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("aminal_image_pkey", x => x.id);
                    table.ForeignKey(
                        name: "animal_image_id_fkey",
                        column: x => x.animal_profile_id,
                        principalTable: "animal_profile",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_animal_type_id",
                table: "animal",
                column: "type_id");

            migrationBuilder.CreateIndex(
                name: "IX_animal_user_id",
                table: "animal",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_animal_image_animal_profile_id",
                table: "animal_image",
                column: "animal_profile_id");

            migrationBuilder.CreateIndex(
                name: "IX_animal_profile_animal_id",
                table: "animal_profile",
                column: "animal_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "animal_type_type_name_key",
                table: "animal_type",
                column: "type_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_role_role_id",
                table: "user_role",
                column: "role_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "animal_image");

            migrationBuilder.DropTable(
                name: "sex");

            migrationBuilder.DropTable(
                name: "user_role");

            migrationBuilder.DropTable(
                name: "animal_profile");

            migrationBuilder.DropTable(
                name: "role");

            migrationBuilder.DropTable(
                name: "animal");

            migrationBuilder.DropTable(
                name: "animal_type");

            migrationBuilder.DropTable(
                name: "user_account");
        }
    }
}
