using Microsoft.EntityFrameworkCore.Migrations;

namespace Seawars.DAL.SqlServer.Migrations
{
    public partial class Init_11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Games",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Games");
        }
    }
}
