using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DemoApp.Migrations
{
    /// <inheritdoc />
    public partial class LoginTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserLogins",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    username = table.Column<string>(type: "nvarchar(64)", nullable: false),
                    passwordHash = table.Column<string>(type: "nvarchar(128)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(64)", nullable: false),
                    role = table.Column<string>(type: "nchar(32)", nullable: false),
                    firstname = table.Column<string>(type: "nvarchar(64)", nullable: true),
                    lastname = table.Column<string>(type: "nvarchar(64)", nullable: true),
                    createdAt = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogins", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserLogins");
        }
    }
}
