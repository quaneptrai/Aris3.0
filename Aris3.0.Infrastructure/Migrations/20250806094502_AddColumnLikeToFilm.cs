using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Aris3._0.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnLikeToFilm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Countries_Films_FilmId",
                table: "Countries");

            migrationBuilder.DropIndex(
                name: "IX_Countries_FilmId",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "FilmId",
                table: "Countries");

            migrationBuilder.AddColumn<int>(
                name: "Like",
                table: "Films",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CountryFilm",
                columns: table => new
                {
                    CountriesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FilmsId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CountryFilm", x => new { x.CountriesId, x.FilmsId });
                    table.ForeignKey(
                        name: "FK_CountryFilm_Countries_CountriesId",
                        column: x => x.CountriesId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CountryFilm_Films_FilmsId",
                        column: x => x.FilmsId,
                        principalTable: "Films",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CountryFilm_FilmsId",
                table: "CountryFilm",
                column: "FilmsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CountryFilm");

            migrationBuilder.DropColumn(
                name: "Like",
                table: "Films");

            migrationBuilder.AddColumn<string>(
                name: "FilmId",
                table: "Countries",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Countries_FilmId",
                table: "Countries",
                column: "FilmId");

            migrationBuilder.AddForeignKey(
                name: "FK_Countries_Films_FilmId",
                table: "Countries",
                column: "FilmId",
                principalTable: "Films",
                principalColumn: "Id");
        }
    }
}
