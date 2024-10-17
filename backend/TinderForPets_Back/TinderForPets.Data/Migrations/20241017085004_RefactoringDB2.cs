using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TinderForPets.Data.Migrations
{
    /// <inheritdoc />
    public partial class RefactoringDB2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "UploadDate",
                table: "animal_type",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UploadDate",
                table: "animal_type");
        }
    }
}
