using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TinderForPets.Data.Migrations
{
    /// <inheritdoc />
    public partial class DateOfBirthColumnV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "chk_width_ge_than_0_3",
                table: "animal_profile");

            migrationBuilder.RenameColumn(
                name: "width",
                table: "animal_profile",
                newName: "weight");

            migrationBuilder.AddCheckConstraint(
                name: "chk_weight_ge_than_0_3",
                table: "animal_profile",
                sql: "weight >= 0.3");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "chk_weight_ge_than_0_3",
                table: "animal_profile");

            migrationBuilder.RenameColumn(
                name: "weight",
                table: "animal_profile",
                newName: "width");

            migrationBuilder.AddCheckConstraint(
                name: "chk_width_ge_than_0_3",
                table: "animal_profile",
                sql: "width >= 0.3");
        }
    }
}
