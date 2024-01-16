using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameSite.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexToTypeId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Posts_TypeId",
                table: "Posts",
                column: "TypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Posts_TypeId",
                table: "Posts");
        }
    }
}
