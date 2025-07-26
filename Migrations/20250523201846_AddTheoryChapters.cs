using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Drivia.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddTheoryChapters : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChapterProgress",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ChapterId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsCompleted = table.Column<bool>(type: "boolean", nullable: false),
                    LastViewedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChapterProgress", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Chapters",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    Summary = table.Column<string>(type: "text", nullable: false),
                    Notes = table.Column<string>(type: "text", nullable: false),
                    Order = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chapters", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChapterProgress");

            migrationBuilder.DropTable(
                name: "Chapters");
        }
    }
}
