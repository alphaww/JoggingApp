using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JoggingApp.EntityFramework.Migrations
{
    public partial class IncludedEmailIdxProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_User_Email",
                table: "User");

            migrationBuilder.CreateIndex(
                name: "IX_User_Email",
                table: "User",
                column: "Email",
                unique: true)
                .Annotation("SqlServer:Include", new[] { "Id", "Password", "State" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_User_Email",
                table: "User");

            migrationBuilder.CreateIndex(
                name: "IX_User_Email",
                table: "User",
                column: "Email",
                unique: true);
        }
    }
}
