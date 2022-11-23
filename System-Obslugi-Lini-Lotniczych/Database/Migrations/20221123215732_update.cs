using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LotSystem.Migrations
{
    public partial class update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "Flags",
                table: "users",
                type: "INTEGER",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.CreateTable(
                name: "users_sessions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ExpireTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsExpiered = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users_sessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_users_sessions_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_users_sessions_UserId",
                table: "users_sessions",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "users_sessions");

            migrationBuilder.DropColumn(
                name: "Flags",
                table: "users");
        }
    }
}
