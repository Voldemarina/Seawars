using Microsoft.EntityFrameworkCore.Migrations;

namespace Seawars.DAL.SqlServer.Migrations
{
    public partial class Init_10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Move",
                table: "Steps",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Move",
                table: "Steps");
        }
    }
}
