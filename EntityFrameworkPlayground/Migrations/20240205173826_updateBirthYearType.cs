using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EntityFrameworkPlayground.Migrations
{
    /// <inheritdoc />
    public partial class updateBirthYearType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<ushort>(
                name: "BirthYear",
                table: "Actors",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "BirthYear",
                table: "Actors",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(ushort),
                oldType: "INTEGER");
        }
    }
}
