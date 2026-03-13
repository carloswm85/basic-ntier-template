using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BasicNtierTemplate.Data.Migrations
{
    /// <inheritdoc />
    public partial class ImgUrlAddition : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImgUrl",
                table: "Student",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImgUrlLocal",
                table: "Student",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImgUrl",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "ImgUrlLocal",
                table: "Student");
        }
    }
}
