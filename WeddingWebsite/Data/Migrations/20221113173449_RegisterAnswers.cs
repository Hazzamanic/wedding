using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeddingWebsite.Data.Migrations
{
    public partial class RegisterAnswers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Guest1Brunch",
                table: "AspNetUsers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Guest1DietaryRequirements",
                table: "AspNetUsers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Guest1IsAttending",
                table: "AspNetUsers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Guest1PizzaParty",
                table: "AspNetUsers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Guest1SongRequest",
                table: "AspNetUsers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Guest2Brunch",
                table: "AspNetUsers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Guest2DietaryRequirements",
                table: "AspNetUsers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Guest2IsAttending",
                table: "AspNetUsers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Guest2PizzaParty",
                table: "AspNetUsers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Guest2SongRequest",
                table: "AspNetUsers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MoreInfoRequest",
                table: "AspNetUsers",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Guest1Brunch",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Guest1DietaryRequirements",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Guest1IsAttending",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Guest1PizzaParty",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Guest1SongRequest",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Guest2Brunch",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Guest2DietaryRequirements",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Guest2IsAttending",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Guest2PizzaParty",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Guest2SongRequest",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "MoreInfoRequest",
                table: "AspNetUsers");
        }
    }
}
