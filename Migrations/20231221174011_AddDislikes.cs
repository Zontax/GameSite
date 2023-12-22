using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameSite.Migrations
{
    /// <inheritdoc />
    public partial class AddDislikes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PostUserDislikes",
                columns: table => new
                {
                    DislikedByUsersId = table.Column<string>(type: "TEXT", nullable: false),
                    DislikedPostsId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostUserDislikes", x => new { x.DislikedByUsersId, x.DislikedPostsId });
                    table.ForeignKey(
                        name: "FK_PostUserDislikes_AspNetUsers_DislikedByUsersId",
                        column: x => x.DislikedByUsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostUserDislikes_Posts_DislikedPostsId",
                        column: x => x.DislikedPostsId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PostUserDislikes_DislikedPostsId",
                table: "PostUserDislikes",
                column: "DislikedPostsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PostUserDislikes");
        }
    }
}
