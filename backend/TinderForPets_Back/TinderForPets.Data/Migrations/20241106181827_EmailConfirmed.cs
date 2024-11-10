using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TinderForPets.Data.Migrations
{
    /// <inheritdoc />
    public partial class EmailConfirmed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "chk_height_ge_than_10",
                table: "animal_profile");

            migrationBuilder.DropCheckConstraint(
                name: "chk_weight_ge_than_0_3",
                table: "animal_profile");

            migrationBuilder.AddColumn<bool>(
                name: "email_confirmed",
                table: "user_account",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddCheckConstraint(
                name: "chk_height_ge_than_10",
                table: "animal_profile",
                sql: "height IS NULL OR height >= 10");

            migrationBuilder.AddCheckConstraint(
                name: "chk_weight_ge_than_0_3",
                table: "animal_profile",
                sql: "weight IS NULL OR weight >= 0.3");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "chk_height_ge_than_10",
                table: "animal_profile");

            migrationBuilder.DropCheckConstraint(
                name: "chk_weight_ge_than_0_3",
                table: "animal_profile");

            migrationBuilder.DropColumn(
                name: "email_confirmed",
                table: "user_account");

            migrationBuilder.AddCheckConstraint(
                name: "chk_height_ge_than_10",
                table: "animal_profile",
                sql: "height>=10");

            migrationBuilder.AddCheckConstraint(
                name: "chk_weight_ge_than_0_3",
                table: "animal_profile",
                sql: "weight >= 0.3");
        }
    }
}
