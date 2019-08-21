using Microsoft.EntityFrameworkCore.Migrations;

namespace LevelCounter.Migrations
{
    public partial class addIsPublicProp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "AspNetUsers");
        }
    }
}
