using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CIS341_project.Migrations
{
    /// <inheritdoc />
    public partial class InitialReactionImplement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reactions_Accounts_ReactionAuthorId",
                table: "Reactions");

            migrationBuilder.DropIndex(
                name: "IX_Reactions_ReactionAuthorId",
                table: "Reactions");

            migrationBuilder.DropColumn(
                name: "ReactionAuthorId",
                table: "Reactions");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Reactions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Reactions");

            migrationBuilder.AddColumn<int>(
                name: "ReactionAuthorId",
                table: "Reactions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Reactions_ReactionAuthorId",
                table: "Reactions",
                column: "ReactionAuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reactions_Accounts_ReactionAuthorId",
                table: "Reactions",
                column: "ReactionAuthorId",
                principalTable: "Accounts",
                principalColumn: "AccountId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
