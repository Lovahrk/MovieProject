using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieProject.Server.Migrations
{
    /// <inheritdoc />
    public partial class RemovedSecondName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SecondName",
                table: "Director");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SecondName",
                table: "Director",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
