using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace proj.Data.Migrations
{
    public partial class userss : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Role",
                table: "AspNetUsers",
                newName: "Pronouns");

            migrationBuilder.AddColumn<string>(
                name: "UserCustomId",
                table: "Comments",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Bio",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPrivate",
                table: "AspNetUsers",
                type: "bit",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserCustomId",
                table: "Comments",
                column: "UserCustomId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_AspNetUsers_UserCustomId",
                table: "Comments",
                column: "UserCustomId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_AspNetUsers_UserCustomId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_UserCustomId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "UserCustomId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "Bio",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsPrivate",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "Pronouns",
                table: "AspNetUsers",
                newName: "Role");
        }
    }
}
