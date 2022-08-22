using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeddingWebsite.Data.Migrations
{
    public partial class GuestEmail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GuestEmail",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserEditViewModel",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    GuestName = table.Column<string>(type: "text", nullable: false),
                    GroupName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserEditViewModel", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserEditViewModel");

            migrationBuilder.DropColumn(
                name: "GuestEmail",
                table: "AspNetUsers");
        }
    }
}
