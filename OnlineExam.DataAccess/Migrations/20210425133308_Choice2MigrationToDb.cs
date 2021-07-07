using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineExam.DataAccess.Migrations
{
    public partial class Choice2MigrationToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsTrue",
                table: "Choices");

            migrationBuilder.AddColumn<int>(
                name: "ChoiceNumber",
                table: "Choices",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChoiceNumber",
                table: "Choices");

            migrationBuilder.AddColumn<bool>(
                name: "IsTrue",
                table: "Choices",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
