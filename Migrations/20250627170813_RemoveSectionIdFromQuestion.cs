using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Drivia.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveSectionIdFromQuestion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Sections_SectionId",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Questions_SectionId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "SectionId",
                table: "Questions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SectionId",
                table: "Questions",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Questions_SectionId",
                table: "Questions",
                column: "SectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Sections_SectionId",
                table: "Questions",
                column: "SectionId",
                principalTable: "Sections",
                principalColumn: "Id");
        }
    }
}
