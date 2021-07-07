using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineExam.DataAccess.Migrations
{
    public partial class QuestionResultMigrationToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QuestionResults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChoiceSelected = table.Column<int>(type: "int", nullable: false),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    ExamResultId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionResults_ExamResults_ExamResultId",
                        column: x => x.ExamResultId,
                        principalTable: "ExamResults",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuestionResults_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuestionResults_ExamResultId",
                table: "QuestionResults",
                column: "ExamResultId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionResults_QuestionId",
                table: "QuestionResults",
                column: "QuestionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuestionResults");
        }
    }
}
