using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameSite.Migrations
{
    /// <inheritdoc />
    public partial class FixPost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReviewGameId",
                table: "Posts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReviewGameId",
                table: "Posts",
                type: "TEXT",
                nullable: true);
        }
    }
}
