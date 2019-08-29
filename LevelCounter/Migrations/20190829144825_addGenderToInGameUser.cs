using Microsoft.EntityFrameworkCore.Migrations;

namespace LevelCounter.Migrations
{
    public partial class addGenderToInGameUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InGameUser_Games_GameId",
                table: "InGameUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InGameUser",
                table: "InGameUser");

            migrationBuilder.RenameTable(
                name: "InGameUser",
                newName: "InGameUsers");

            migrationBuilder.RenameIndex(
                name: "IX_InGameUser_GameId",
                table: "InGameUsers",
                newName: "IX_InGameUsers_GameId");

            migrationBuilder.AddColumn<int>(
                name: "Gender",
                table: "InGameUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Sex",
                table: "InGameUsers",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_InGameUsers",
                table: "InGameUsers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InGameUsers_Games_GameId",
                table: "InGameUsers",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "GameId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InGameUsers_Games_GameId",
                table: "InGameUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InGameUsers",
                table: "InGameUsers");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "InGameUsers");

            migrationBuilder.DropColumn(
                name: "Sex",
                table: "InGameUsers");

            migrationBuilder.RenameTable(
                name: "InGameUsers",
                newName: "InGameUser");

            migrationBuilder.RenameIndex(
                name: "IX_InGameUsers_GameId",
                table: "InGameUser",
                newName: "IX_InGameUser_GameId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InGameUser",
                table: "InGameUser",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InGameUser_Games_GameId",
                table: "InGameUser",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "GameId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
