using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TinderForPets.Data.Migrations
{
    /// <inheritdoc />
    public partial class S2CellIdAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "s2_cell_id",
                table: "animal_profile",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "ix_s2_cell_id",
                table: "animal_profile",
                column: "s2_cell_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_s2_cell_id",
                table: "animal_profile");

            migrationBuilder.DropColumn(
                name: "s2_cell_id",
                table: "animal_profile");
        }
    }
}
