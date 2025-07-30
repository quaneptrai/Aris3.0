using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Aris3._0.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangingConfigure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Actors_Films_FilmId",
                table: "Actors");

            migrationBuilder.DropIndex(
                name: "IX_Actors_FilmId",
                table: "Actors");

            migrationBuilder.DropColumn(
                name: "FilmId",
                table: "Actors");

            migrationBuilder.CreateTable(
                name: "ActorFilms",
                columns: table => new
                {
                    Actorsid = table.Column<int>(type: "int", nullable: false),
                    FilmsId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActorFilms", x => new { x.Actorsid, x.FilmsId });
                    table.ForeignKey(
                        name: "FK_ActorFilms_Actors_Actorsid",
                        column: x => x.Actorsid,
                        principalTable: "Actors",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActorFilms_Films_FilmsId",
                        column: x => x.FilmsId,
                        principalTable: "Films",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActorFilms_FilmsId",
                table: "ActorFilms",
                column: "FilmsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActorFilms");

            migrationBuilder.AddColumn<string>(
                name: "FilmId",
                table: "Actors",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Actors_FilmId",
                table: "Actors",
                column: "FilmId");

            migrationBuilder.AddForeignKey(
                name: "FK_Actors_Films_FilmId",
                table: "Actors",
                column: "FilmId",
                principalTable: "Films",
                principalColumn: "Id");
        }
    }
}
