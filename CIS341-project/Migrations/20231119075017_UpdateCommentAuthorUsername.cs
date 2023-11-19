using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CIS341_project.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCommentAuthorUsername : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Accounts_AuthorAccountId1",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_AuthorAccountId1",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "AuthorAccountId1",
                table: "Comments");

            migrationBuilder.RenameColumn(
                name: "AuthorAccountId",
                table: "Comments",
                newName: "AuthorUsername");

            migrationBuilder.AlterColumn<int>(
                name: "ParentCommentId",
                table: "Comments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "AuthorId",
                table: "Comments",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AuthorUsername",
                table: "Comments",
                newName: "AuthorAccountId");

            migrationBuilder.AlterColumn<int>(
                name: "ParentCommentId",
                table: "Comments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AuthorId",
                table: "Comments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AuthorAccountId1",
                table: "Comments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_AuthorAccountId1",
                table: "Comments",
                column: "AuthorAccountId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Accounts_AuthorAccountId1",
                table: "Comments",
                column: "AuthorAccountId1",
                principalTable: "Accounts",
                principalColumn: "AccountId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
