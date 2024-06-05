using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace proj.Data.Migrations
{
    public partial class boolmigr : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsExternal",
                table: "Articles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Link",
                table: "Articles",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsExternal",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "Link",
                table: "Articles");
        }
    }
}
