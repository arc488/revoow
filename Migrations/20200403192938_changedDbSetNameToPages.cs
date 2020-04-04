using Microsoft.EntityFrameworkCore.Migrations;

namespace Revoow.Migrations
{
    public partial class changedDbSetNameToPages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Testimonials_LandingPages_PageId",
                table: "Testimonials");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LandingPages",
                table: "LandingPages");

            migrationBuilder.RenameTable(
                name: "LandingPages",
                newName: "Pages");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Pages",
                table: "Pages",
                column: "PageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Testimonials_Pages_PageId",
                table: "Testimonials",
                column: "PageId",
                principalTable: "Pages",
                principalColumn: "PageId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Testimonials_Pages_PageId",
                table: "Testimonials");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Pages",
                table: "Pages");

            migrationBuilder.RenameTable(
                name: "Pages",
                newName: "LandingPages");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LandingPages",
                table: "LandingPages",
                column: "PageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Testimonials_LandingPages_PageId",
                table: "Testimonials",
                column: "PageId",
                principalTable: "LandingPages",
                principalColumn: "PageId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
