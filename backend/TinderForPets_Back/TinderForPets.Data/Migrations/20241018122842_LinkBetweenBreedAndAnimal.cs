using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TinderForPets.Data.Migrations
{
    /// <inheritdoc />
    public partial class LinkBetweenBreedAndAnimal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "breed_id",
                table: "animal",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_animal_breed_id",
                table: "animal",
                column: "breed_id");

            migrationBuilder.AddForeignKey(
                name: "animal_breed_id_fkey",
                table: "animal",
                column: "breed_id",
                principalTable: "breed",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "animal_breed_id_fkey",
                table: "animal");

            migrationBuilder.DropIndex(
                name: "IX_animal_breed_id",
                table: "animal");

            migrationBuilder.DropColumn(
                name: "breed_id",
                table: "animal");
        }
    }
}
