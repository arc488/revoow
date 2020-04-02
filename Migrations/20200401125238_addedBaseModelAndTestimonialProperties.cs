using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Revoow.Migrations
{
    public partial class addedBaseModelAndTestimonialProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Testimonials",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "Testimonials",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReviewerName",
                table: "Testimonials",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TimeCreated",
                table: "Testimonials",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "TimeModified",
                table: "Testimonials",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<byte[]>(
                name: "VideoThumbnail",
                table: "Testimonials",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "LandingPages",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "LandingPages",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TimeCreated",
                table: "LandingPages",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "TimeModified",
                table: "LandingPages",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Testimonials");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Testimonials");

            migrationBuilder.DropColumn(
                name: "ReviewerName",
                table: "Testimonials");

            migrationBuilder.DropColumn(
                name: "TimeCreated",
                table: "Testimonials");

            migrationBuilder.DropColumn(
                name: "TimeModified",
                table: "Testimonials");

            migrationBuilder.DropColumn(
                name: "VideoThumbnail",
                table: "Testimonials");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "LandingPages");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "LandingPages");

            migrationBuilder.DropColumn(
                name: "TimeCreated",
                table: "LandingPages");

            migrationBuilder.DropColumn(
                name: "TimeModified",
                table: "LandingPages");
        }
    }
}
