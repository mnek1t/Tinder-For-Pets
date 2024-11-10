using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TinderForPets.Data.Migrations
{
    /// <inheritdoc />
    public partial class AnimalTableColumnModification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "type_id",
                table: "animal",
                newName: "animal_type_id");

            migrationBuilder.RenameIndex(
                name: "IX_animal_type_id",
                table: "animal",
                newName: "IX_animal_animal_type_id");

            migrationBuilder.AlterColumn<double>(
                name: "longitude",
                table: "animal_profile",
                type: "double precision",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<double>(
                name: "latitude",
                table: "animal_profile",
                type: "double precision",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "animal_type_id",
                table: "animal",
                newName: "type_id");

            migrationBuilder.RenameIndex(
                name: "IX_animal_animal_type_id",
                table: "animal",
                newName: "IX_animal_type_id");

            migrationBuilder.AlterColumn<decimal>(
                name: "longitude",
                table: "animal_profile",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AlterColumn<decimal>(
                name: "latitude",
                table: "animal_profile",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision");
        }
    }
}
