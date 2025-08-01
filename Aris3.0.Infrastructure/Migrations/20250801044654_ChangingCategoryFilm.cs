using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Aris3._0.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangingCategoryFilm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FilmCategories_Films_FilmsId",
                table: "FilmCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_FilmCategories_categories_CategoriesId",
                table: "FilmCategories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FilmCategories",
                table: "FilmCategories");

            migrationBuilder.RenameTable(
                name: "FilmCategories",
                newName: "CategoryFilm");

            migrationBuilder.RenameIndex(
                name: "IX_FilmCategories_FilmsId",
                table: "CategoryFilm",
                newName: "IX_CategoryFilm_FilmsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CategoryFilm",
                table: "CategoryFilm",
                columns: new[] { "CategoriesId", "FilmsId" });

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryFilm_Films_FilmsId",
                table: "CategoryFilm",
                column: "FilmsId",
                principalTable: "Films",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryFilm_categories_CategoriesId",
                table: "CategoryFilm",
                column: "CategoriesId",
                principalTable: "categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryFilm_Films_FilmsId",
                table: "CategoryFilm");

            migrationBuilder.DropForeignKey(
                name: "FK_CategoryFilm_categories_CategoriesId",
                table: "CategoryFilm");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CategoryFilm",
                table: "CategoryFilm");

            migrationBuilder.RenameTable(
                name: "CategoryFilm",
                newName: "FilmCategories");

            migrationBuilder.RenameIndex(
                name: "IX_CategoryFilm_FilmsId",
                table: "FilmCategories",
                newName: "IX_FilmCategories_FilmsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FilmCategories",
                table: "FilmCategories",
                columns: new[] { "CategoriesId", "FilmsId" });

            migrationBuilder.AddForeignKey(
                name: "FK_FilmCategories_Films_FilmsId",
                table: "FilmCategories",
                column: "FilmsId",
                principalTable: "Films",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FilmCategories_categories_CategoriesId",
                table: "FilmCategories",
                column: "CategoriesId",
                principalTable: "categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
