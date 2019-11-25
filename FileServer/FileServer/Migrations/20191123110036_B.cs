using Microsoft.EntityFrameworkCore.Migrations;

namespace FileServer.Migrations
{
    public partial class B : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "FileDatas");

            migrationBuilder.AddColumn<string>(
                name: "IP",
                table: "FileDatas",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IP",
                table: "FileDatas");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "FileDatas",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
