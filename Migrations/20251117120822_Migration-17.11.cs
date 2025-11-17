using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cloud.Migrations
{
    /// <inheritdoc />
    public partial class Migration1711 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "extension",
                table: "Files",
                newName: "extention");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "extention",
                table: "Files",
                newName: "extension");
        }
    }
}
