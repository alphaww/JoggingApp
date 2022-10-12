using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JoggingApp.EntityFramework.Migrations
{
    public partial class OutboxEventState : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EventState",
                table: "OutboxMessage",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EventState",
                table: "OutboxMessage");
        }
    }
}
