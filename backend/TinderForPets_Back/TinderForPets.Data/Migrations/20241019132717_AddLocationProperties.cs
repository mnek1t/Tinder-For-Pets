using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TinderForPets.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddLocationProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "chk_age_ge_than_1",
                table: "animal_profile");

            migrationBuilder.AlterColumn<decimal>(
                name: "age",
                table: "animal_profile",
                type: "numeric(2,0)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<string>(
                name: "city",
                table: "animal_profile",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "country",
                table: "animal_profile",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "height",
                table: "animal_profile",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "latitude",
                table: "animal_profile",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "longitude",
                table: "animal_profile",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "width",
                table: "animal_profile",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddCheckConstraint(
                name: "chk_age_ge_than_0_3",
                table: "animal_profile",
                sql: "width >= 0.3");

            migrationBuilder.AddCheckConstraint(
                name: "chk_age_ge_than_1",
                table: "animal_profile",
                sql: "age >= 1");

            migrationBuilder.AddCheckConstraint(
                name: "chk_height_ge_than_10",
                table: "animal_profile",
                sql: "height>=10");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "chk_age_ge_than_0_3",
                table: "animal_profile");

            migrationBuilder.DropCheckConstraint(
                name: "chk_age_ge_than_1",
                table: "animal_profile");

            migrationBuilder.DropCheckConstraint(
                name: "chk_height_ge_than_10",
                table: "animal_profile");

            migrationBuilder.DropColumn(
                name: "city",
                table: "animal_profile");

            migrationBuilder.DropColumn(
                name: "country",
                table: "animal_profile");

            migrationBuilder.DropColumn(
                name: "height",
                table: "animal_profile");

            migrationBuilder.DropColumn(
                name: "latitude",
                table: "animal_profile");

            migrationBuilder.DropColumn(
                name: "longitude",
                table: "animal_profile");

            migrationBuilder.DropColumn(
                name: "width",
                table: "animal_profile");

            migrationBuilder.AlterColumn<int>(
                name: "age",
                table: "animal_profile",
                type: "integer",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(2,0)");

            migrationBuilder.AddCheckConstraint(
                name: "chk_age_ge_than_1",
                table: "animal_profile",
                sql: "[age] => 1");
        }
    }
}
