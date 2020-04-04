using Microsoft.EntityFrameworkCore.Migrations;

namespace Revoow.Migrations
{
    public partial class addedPropertiesToTestimonial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VideoPath",
                table: "Testimonials",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VideoPath",
                table: "Testimonials");
        }
    }
}
