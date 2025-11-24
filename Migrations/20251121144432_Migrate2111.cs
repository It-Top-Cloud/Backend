using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cloud.Migrations
{
    /// <inheritdoc />
    public partial class Migrate2111 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "shared_files",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    file_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    сreated_at = table.Column<DateTime>(type: "datetime", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shared_files", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "FileSharedFile",
                columns: table => new
                {
                    Fileid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SharedFileid = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileSharedFile", x => new { x.Fileid, x.SharedFileid });
                    table.ForeignKey(
                        name: "FK_FileSharedFile_Files_Fileid",
                        column: x => x.Fileid,
                        principalTable: "Files",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FileSharedFile_shared_files_SharedFileid",
                        column: x => x.SharedFileid,
                        principalTable: "shared_files",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FileSharedFile_SharedFileid",
                table: "FileSharedFile",
                column: "SharedFileid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileSharedFile");

            migrationBuilder.DropTable(
                name: "shared_files");
        }
    }
}
