using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class FixRelaQuestion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuestionInMatchs_Questions_QuestionId",
                table: "QuestionInMatchs");

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionInMatchs_Questions_QuestionId",
                table: "QuestionInMatchs",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuestionInMatchs_Questions_QuestionId",
                table: "QuestionInMatchs");

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionInMatchs_Questions_QuestionId",
                table: "QuestionInMatchs",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
