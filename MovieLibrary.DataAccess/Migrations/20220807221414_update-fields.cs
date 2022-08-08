using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieLibrary.DataAccess.Migrations
{
    public partial class updatefields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Comments");

            migrationBuilder.AddColumn<string>(
                name: "UserNickname",
                table: "Ratings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserNickname",
                table: "Comments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserNickname",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "UserNickname",
                table: "Comments");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Ratings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Comments",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
