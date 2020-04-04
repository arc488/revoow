using Microsoft.EntityFrameworkCore.Migrations;

namespace Revoow.Migrations
{
    public partial class addedTestimonialListToPage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Testimonials_PageId",
                table: "Testimonials",
                column: "PageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Testimonials_LandingPages_PageId",
                table: "Testimonials",
                column: "PageId",
                principalTable: "LandingPages",
                principalColumn: "PageId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Testimonials_LandingPages_PageId",
                table: "Testimonials");

            migrationBuilder.DropIndex(
                name: "IX_Testimonials_PageId",
                table: "Testimonials");
        }
    }
}
