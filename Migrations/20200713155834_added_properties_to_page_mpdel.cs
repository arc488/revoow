using Microsoft.EntityFrameworkCore.Migrations;

namespace Revoow.Migrations
{
    public partial class added_properties_to_page_mpdel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Pages",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Pages",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Pages",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WebsiteUrl",
                table: "Pages",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Pages");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Pages");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Pages");

            migrationBuilder.DropColumn(
                name: "WebsiteUrl",
                table: "Pages");
        }
    }
}
