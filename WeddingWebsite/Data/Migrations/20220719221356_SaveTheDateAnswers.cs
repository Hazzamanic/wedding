using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeddingWebsite.Data.Migrations
{
    public partial class SaveTheDateAnswers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SaveTheDateAnswer",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SaveTheDateGuestAnswer",
                table: "AspNetUsers",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SaveTheDateAnswer",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SaveTheDateGuestAnswer",
                table: "AspNetUsers");
        }
    }
}
