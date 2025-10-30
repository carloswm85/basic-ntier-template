using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BasicNtierTemplate.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigrationWithoutIdentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Blog",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    url = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blog", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Posteo",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    titulo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    contenido = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    blogid = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posteo", x => x.id);
                    table.ForeignKey(
                        name: "FK_Posteo_Blog_blogid",
                        column: x => x.blogid,
                        principalTable: "Blog",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Blog",
                columns: new[] { "id", "url" },
                values: new object[,]
                {
                    { 1, "https://example.com/blog1" },
                    { 2, "https://example.com/blog2" },
                    { 3, "https://example.com/blog3" },
                    { 4, "https://example.com/blog4" }
                });

            migrationBuilder.InsertData(
                table: "Posteo",
                columns: new[] { "id", "blogid", "contenido", "titulo" },
                values: new object[,]
                {
                    { 1, 1, "Content for Post 1", "Post 1" },
                    { 2, 1, "Content for Post 2", "Post 2" },
                    { 3, 2, "Content for Post A", "Post A" },
                    { 4, 3, "Content for Post B", "Post B" },
                    { 5, 4, "Content for Post C", "Post C" },
                    { 6, 4, "Content for Post D", "Post D" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Posteo_blogid",
                table: "Posteo",
                column: "blogid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Posteo");

            migrationBuilder.DropTable(
                name: "Blog");
        }
    }
}
