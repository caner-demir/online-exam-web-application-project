using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineExam.DataAccess.Migrations
{
    public partial class Question7MigrationToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CorrectChoiceNumber",
                table: "Questions");

            migrationBuilder.AddColumn<int>(
                name: "CorrectChoice",
                table: "Questions",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CorrectChoice",
                table: "Questions");

            migrationBuilder.AddColumn<int>(
                name: "CorrectChoiceNumber",
                table: "Questions",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
