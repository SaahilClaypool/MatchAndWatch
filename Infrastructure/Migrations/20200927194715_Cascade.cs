using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations {
  public partial class Cascade : Migration {
    protected override void Up(MigrationBuilder migrationBuilder) {
      migrationBuilder.CreateIndex(
          name: "IX_Genre_TitleId",
          table: "Genre",
          column: "TitleId");

      migrationBuilder.AddForeignKey(
          name: "FK_Genre_TitleAggs_TitleId",
          table: "Genre",
          column: "TitleId",
          principalTable: "TitleAggs",
          principalColumn: "Id",
          onDelete: ReferentialAction.Cascade);
    }

    protected override void Down(MigrationBuilder migrationBuilder) {
      migrationBuilder.DropForeignKey(
          name: "FK_Genre_TitleAggs_TitleId",
          table: "Genre");

      migrationBuilder.DropIndex(
          name: "IX_Genre_TitleId",
          table: "Genre");
    }
  }
}
