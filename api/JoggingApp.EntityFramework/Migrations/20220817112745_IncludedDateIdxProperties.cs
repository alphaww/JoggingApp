using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JoggingApp.EntityFramework.Migrations
{
    public partial class IncludedDateIdxProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Jog_Date",
                table: "Jog");

            migrationBuilder.CreateIndex(
                name: "IX_Jog_Date",
                table: "Jog",
                column: "Date")
                .Annotation("SqlServer:Include", new[] { "Id", "Distance", "Time", "UserId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Jog_Date",
                table: "Jog");

            migrationBuilder.CreateIndex(
                name: "IX_Jog_Date",
                table: "Jog",
                column: "Date");
        }
    }
}
