using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeddingWebsite.Data.Migrations
{
    public partial class SaveTheDateEmailMarker : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasSentGuestSaveTheDateEmail",
                table: "AspNetUsers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasSentSaveTheDateEmail",
                table: "AspNetUsers",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasSentGuestSaveTheDateEmail",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "HasSentSaveTheDateEmail",
                table: "AspNetUsers");
        }
    }
}
