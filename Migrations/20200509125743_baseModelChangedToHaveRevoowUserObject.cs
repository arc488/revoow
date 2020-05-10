using Microsoft.EntityFrameworkCore.Migrations;

namespace Revoow.Migrations
{
    public partial class baseModelChangedToHaveRevoowUserObject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Testimonials");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Testimonials");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Pages");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Pages");

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "Testimonials",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedById",
                table: "Testimonials",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "Pages",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedById",
                table: "Pages",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Testimonials_CreatedById",
                table: "Testimonials",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Testimonials_ModifiedById",
                table: "Testimonials",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Pages_CreatedById",
                table: "Pages",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Pages_ModifiedById",
                table: "Pages",
                column: "ModifiedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Pages_AspNetUsers_CreatedById",
                table: "Pages",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Pages_AspNetUsers_ModifiedById",
                table: "Pages",
                column: "ModifiedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Testimonials_AspNetUsers_CreatedById",
                table: "Testimonials",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Testimonials_AspNetUsers_ModifiedById",
                table: "Testimonials",
                column: "ModifiedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pages_AspNetUsers_CreatedById",
                table: "Pages");

            migrationBuilder.DropForeignKey(
                name: "FK_Pages_AspNetUsers_ModifiedById",
                table: "Pages");

            migrationBuilder.DropForeignKey(
                name: "FK_Testimonials_AspNetUsers_CreatedById",
                table: "Testimonials");

            migrationBuilder.DropForeignKey(
                name: "FK_Testimonials_AspNetUsers_ModifiedById",
                table: "Testimonials");

            migrationBuilder.DropIndex(
                name: "IX_Testimonials_CreatedById",
                table: "Testimonials");

            migrationBuilder.DropIndex(
                name: "IX_Testimonials_ModifiedById",
                table: "Testimonials");

            migrationBuilder.DropIndex(
                name: "IX_Pages_CreatedById",
                table: "Pages");

            migrationBuilder.DropIndex(
                name: "IX_Pages_ModifiedById",
                table: "Pages");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Testimonials");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "Testimonials");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Pages");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "Pages");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Testimonials",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "Testimonials",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Pages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "Pages",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
