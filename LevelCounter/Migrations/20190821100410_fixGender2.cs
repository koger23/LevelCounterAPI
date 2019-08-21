using Microsoft.EntityFrameworkCore.Migrations;

namespace LevelCounter.Migrations
{
    public partial class fixGender2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Sex",
                table: "AspNetUsers",
                nullable: false,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Sex",
                table: "AspNetUsers",
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
