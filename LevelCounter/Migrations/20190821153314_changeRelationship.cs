using Microsoft.EntityFrameworkCore.Migrations;

namespace LevelCounter.Migrations
{
    public partial class changeRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Relationship_AspNetUsers_RelatingUserId",
                table: "Relationship");

            migrationBuilder.DropForeignKey(
                name: "FK_Relationship_AspNetUsers_UserId",
                table: "Relationship");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRelationships_Relationship_RelationshipId",
                table: "UserRelationships");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Relationship",
                table: "Relationship");

            migrationBuilder.RenameTable(
                name: "Relationship",
                newName: "Relationships");

            migrationBuilder.RenameIndex(
                name: "IX_Relationship_UserId",
                table: "Relationships",
                newName: "IX_Relationships_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Relationship_RelatingUserId",
                table: "Relationships",
                newName: "IX_Relationships_RelatingUserId");

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "Relationships",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Relationships",
                table: "Relationships",
                column: "RelationshipId");

            migrationBuilder.AddForeignKey(
                name: "FK_Relationships_AspNetUsers_RelatingUserId",
                table: "Relationships",
                column: "RelatingUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Relationships_AspNetUsers_UserId",
                table: "Relationships",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRelationships_Relationships_RelationshipId",
                table: "UserRelationships",
                column: "RelationshipId",
                principalTable: "Relationships",
                principalColumn: "RelationshipId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Relationships_AspNetUsers_RelatingUserId",
                table: "Relationships");

            migrationBuilder.DropForeignKey(
                name: "FK_Relationships_AspNetUsers_UserId",
                table: "Relationships");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRelationships_Relationships_RelationshipId",
                table: "UserRelationships");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Relationships",
                table: "Relationships");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Relationships");

            migrationBuilder.RenameTable(
                name: "Relationships",
                newName: "Relationship");

            migrationBuilder.RenameIndex(
                name: "IX_Relationships_UserId",
                table: "Relationship",
                newName: "IX_Relationship_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Relationships_RelatingUserId",
                table: "Relationship",
                newName: "IX_Relationship_RelatingUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Relationship",
                table: "Relationship",
                column: "RelationshipId");

            migrationBuilder.AddForeignKey(
                name: "FK_Relationship_AspNetUsers_RelatingUserId",
                table: "Relationship",
                column: "RelatingUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Relationship_AspNetUsers_UserId",
                table: "Relationship",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRelationships_Relationship_RelationshipId",
                table: "UserRelationships",
                column: "RelationshipId",
                principalTable: "Relationship",
                principalColumn: "RelationshipId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
