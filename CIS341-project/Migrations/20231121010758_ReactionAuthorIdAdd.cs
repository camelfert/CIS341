using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CIS341_project.Migrations
{
    /// <inheritdoc />
    public partial class ReactionAuthorIdAdd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Reactions",
                newName: "ReactionAuthorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReactionAuthorId",
                table: "Reactions",
                newName: "UserId");
        }
    }
}
