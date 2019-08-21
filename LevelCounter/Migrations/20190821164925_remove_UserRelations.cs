using Microsoft.EntityFrameworkCore.Migrations;

namespace LevelCounter.Migrations
{
    public partial class remove_UserRelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserRelationships");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserRelationships",
                columns: table => new
                {
                    ApplicationUserId = table.Column<string>(nullable: false),
                    RelationshipId = table.Column<int>(nullable: false),
                    id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRelationships", x => new { x.ApplicationUserId, x.RelationshipId });
                    table.ForeignKey(
                        name: "FK_UserRelationships_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRelationships_Relationships_RelationshipId",
                        column: x => x.RelationshipId,
                        principalTable: "Relationships",
                        principalColumn: "RelationshipId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserRelationships_RelationshipId",
                table: "UserRelationships",
                column: "RelationshipId");
        }
    }
}
