using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseApp2._0.Data.Migrations
{
    public partial class fourthsetup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Sample",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Sample");
        }
    }
}
