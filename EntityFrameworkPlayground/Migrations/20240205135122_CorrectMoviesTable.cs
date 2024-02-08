using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EntityFrameworkPlayground.Migrations
{
    /// <inheritdoc />
    public partial class CorrectMoviesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Blogs_MovieId",
                table: "Posts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Blogs",
                table: "Blogs");

            migrationBuilder.RenameTable(
                name: "Blogs",
                newName: "Movies");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Movies",
                table: "Movies",
                column: "MovieId");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Movies_MovieId",
                table: "Posts",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "MovieId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Movies_MovieId",
                table: "Posts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Movies",
                table: "Movies");

            migrationBuilder.RenameTable(
                name: "Movies",
                newName: "Blogs");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Blogs",
                table: "Blogs",
                column: "MovieId");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Blogs_MovieId",
                table: "Posts",
                column: "MovieId",
                principalTable: "Blogs",
                principalColumn: "MovieId");
        }
    }
}
