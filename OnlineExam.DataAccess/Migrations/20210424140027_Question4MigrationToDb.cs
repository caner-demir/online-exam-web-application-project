using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineExam.DataAccess.Migrations
{
    public partial class Question4MigrationToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CorrectChoiceId",
                table: "Questions",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CorrectChoiceId",
                table: "Questions");
        }
    }
}
