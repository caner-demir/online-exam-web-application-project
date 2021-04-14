using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineExam.DataAccessToDb.Migrations
{
    public partial class Question2MigrationToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Questions",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Questions");
        }
    }
}
