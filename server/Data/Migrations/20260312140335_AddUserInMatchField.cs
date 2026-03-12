using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class AddUserInMatchField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ExpScoreGained",
                table: "UserMatchResultHistories",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RankScoreGained",
                table: "UserMatchResultHistories",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpScoreGained",
                table: "UserMatchResultHistories");

            migrationBuilder.DropColumn(
                name: "RankScoreGained",
                table: "UserMatchResultHistories");
        }
    }
}
