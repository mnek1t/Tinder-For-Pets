using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TinderForPets.Data.Migrations
{
    /// <inheritdoc />
    public partial class MatchTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "match",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    first_swiper_id = table.Column<Guid>(type: "uuid", nullable: false),
                    second_swiper_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("match_pkey", x => x.id);
                    table.ForeignKey(
                        name: "fk_match_first_swiper",
                        column: x => x.first_swiper_id,
                        principalTable: "animal_profile",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_match_second_swiper",
                        column: x => x.second_swiper_id,
                        principalTable: "animal_profile",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_match_first_swiper_id",
                table: "match",
                column: "first_swiper_id");

            migrationBuilder.CreateIndex(
                name: "ix_match_second_swiper_id",
                table: "match",
                column: "second_swiper_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "match");
        }
    }
}
