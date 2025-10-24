using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cloud.Migrations
{
    /// <inheritdoc />
    public partial class Migrate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "phone_verifications",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    check_id = table.Column<string>(type: "varchar(20)", nullable: false),
                    phone = table.Column<string>(type: "varchar(11)", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    сreated_at = table.Column<DateTime>(type: "datetime", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_phone_verifications", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    fname = table.Column<string>(type: "nvarchar(30)", nullable: false),
                    sname = table.Column<string>(type: "nvarchar(30)", nullable: true),
                    lname = table.Column<string>(type: "nvarchar(30)", nullable: true),
                    email = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    phone = table.Column<string>(type: "varchar(11)", nullable: false),
                    password = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    role = table.Column<int>(type: "int", nullable: false),
                    сreated_at = table.Column<DateTime>(type: "datetime", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_users_email",
                table: "users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_phone",
                table: "users",
                column: "phone",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "phone_verifications");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
