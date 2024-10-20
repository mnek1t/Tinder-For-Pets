using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TinderForPets.Data.Migrations
{
    /// <inheritdoc />
    public partial class CascadeAnimalDeletionOnUserDeleted : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "animal_type_id_fkey",
                table: "animal");

            migrationBuilder.DropForeignKey(
                name: "animal_user_id_fkey",
                table: "animal");

            migrationBuilder.DropCheckConstraint(
                name: "chk_age_ge_than_0_3",
                table: "animal_profile");

            migrationBuilder.AddCheckConstraint(
                name: "chk_width_ge_than_0_3",
                table: "animal_profile",
                sql: "width >= 0.3");

            migrationBuilder.AddForeignKey(
                name: "animal_type_id_fkey",
                table: "animal",
                column: "type_id",
                principalTable: "animal_type",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "animal_user_id_fkey",
                table: "animal",
                column: "user_id",
                principalTable: "user_account",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "animal_type_id_fkey",
                table: "animal");

            migrationBuilder.DropForeignKey(
                name: "animal_user_id_fkey",
                table: "animal");

            migrationBuilder.DropCheckConstraint(
                name: "chk_width_ge_than_0_3",
                table: "animal_profile");

            migrationBuilder.AddCheckConstraint(
                name: "chk_age_ge_than_0_3",
                table: "animal_profile",
                sql: "width >= 0.3");

            migrationBuilder.AddForeignKey(
                name: "animal_type_id_fkey",
                table: "animal",
                column: "type_id",
                principalTable: "animal_type",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "animal_user_id_fkey",
                table: "animal",
                column: "user_id",
                principalTable: "user_account",
                principalColumn: "id");
        }
    }
}
