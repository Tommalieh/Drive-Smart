using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DriveSmart.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldsToQuestion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ChapterTitle",
                table: "Questions",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "GroupTitle",
                table: "Questions",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Questions",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChapterTitle",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "GroupTitle",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Questions");
        }
    }
}
