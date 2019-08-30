using Microsoft.EntityFrameworkCore.Migrations;

namespace LevelCounter.Migrations
{
    public partial class fix_sex_column_in_appuser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sex",
                table: "AspNetUsers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Sex",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: "");
        }
    }
}
