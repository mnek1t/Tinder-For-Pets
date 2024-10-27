using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TinderForPets.Data.Migrations
{
    /// <inheritdoc />
    public partial class Sex2AnimalLinkAgeConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_animal_profile_sex_id",
                table: "animal_profile",
                column: "sex_id");

            migrationBuilder.AddCheckConstraint(
                name: "chk_age_ge_than_1",
                table: "animal_profile",
                sql: "age >= 1");

            migrationBuilder.AddForeignKey(
                name: "animal_profile_sex_id_fkey",
                table: "animal_profile",
                column: "sex_id",
                principalTable: "sex",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "animal_profile_sex_id_fkey",
                table: "animal_profile");

            migrationBuilder.DropIndex(
                name: "IX_animal_profile_sex_id",
                table: "animal_profile");

            migrationBuilder.DropCheckConstraint(
                name: "chk_age_ge_than_1",
                table: "animal_profile");
        }
    }
}
