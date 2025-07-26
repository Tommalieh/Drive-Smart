using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Drivia.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddProgressTracking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SectionProgress",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    SectionId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsCompleted = table.Column<bool>(type: "boolean", nullable: false),
                    LastViewedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SectionProgress", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SectionProgress");
        }
    }
}
