using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LevelCounter.Migrations
{
    public partial class changeRelationOfGame : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InGameUserGames");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InGameUser",
                table: "InGameUser");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "InGameUser",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "InGameUser",
                nullable: false,
                defaultValue: 0)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<int>(
                name: "GameId",
                table: "InGameUser",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_InGameUser",
                table: "InGameUser",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_InGameUser_GameId",
                table: "InGameUser",
                column: "GameId");

            migrationBuilder.AddForeignKey(
                name: "FK_InGameUser_Games_GameId",
                table: "InGameUser",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "GameId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InGameUser_Games_GameId",
                table: "InGameUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InGameUser",
                table: "InGameUser");

            migrationBuilder.DropIndex(
                name: "IX_InGameUser_GameId",
                table: "InGameUser");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "InGameUser");

            migrationBuilder.DropColumn(
                name: "GameId",
                table: "InGameUser");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "InGameUser",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddPrimaryKey(
                name: "PK_InGameUser",
                table: "InGameUser",
                column: "UserId");

            migrationBuilder.CreateTable(
                name: "InGameUserGames",
                columns: table => new
                {
                    InGameUserId = table.Column<string>(nullable: false),
                    GameId = table.Column<int>(nullable: false),
                    Id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InGameUserGames", x => new { x.InGameUserId, x.GameId });
                    table.ForeignKey(
                        name: "FK_InGameUserGames_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "GameId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InGameUserGames_InGameUser_InGameUserId",
                        column: x => x.InGameUserId,
                        principalTable: "InGameUser",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InGameUserGames_GameId",
                table: "InGameUserGames",
                column: "GameId");
        }
    }
}
