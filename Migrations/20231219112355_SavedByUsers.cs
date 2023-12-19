using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameSite.Migrations
{
    /// <inheritdoc />
    public partial class SavedByUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PostUserSaved",
                columns: table => new
                {
                    SavedByUsersId = table.Column<string>(type: "TEXT", nullable: false),
                    SavedPostsId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostUserSaved", x => new { x.SavedByUsersId, x.SavedPostsId });
                    table.ForeignKey(
                        name: "FK_PostUserSaved_AspNetUsers_SavedByUsersId",
                        column: x => x.SavedByUsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostUserSaved_Posts_SavedPostsId",
                        column: x => x.SavedPostsId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PostUserSaved_SavedPostsId",
                table: "PostUserSaved",
                column: "SavedPostsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PostUserSaved");
        }
    }
}
