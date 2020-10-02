using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations {
  public partial class UpdateEntities : Migration {
    protected override void Up(MigrationBuilder migrationBuilder) {
      migrationBuilder.CreateTable(
          name: "TitleAggs",
          columns: table => new {
            Id = table.Column<string>(type: "TEXT", nullable: false),
            Type = table.Column<string>(type: "TEXT", nullable: true),
            Name = table.Column<string>(type: "TEXT", nullable: true),
            ReleaseYear = table.Column<int>(type: "INTEGER", nullable: true),
            EndYear = table.Column<int>(type: "INTEGER", nullable: true),
            RunTime = table.Column<int>(type: "INTEGER", nullable: false)
          },
          constraints: table => {
            table.PrimaryKey("PK_TitleAggs", x => x.Id);
          });

      migrationBuilder.CreateTable(
          name: "Genre",
          columns: table => new {
            Id = table.Column<int>(type: "INTEGER", nullable: false)
                  .Annotation("Sqlite:Autoincrement", true),
            TitleId = table.Column<string>(type: "TEXT", nullable: true),
            TitleAggId = table.Column<string>(type: "TEXT", nullable: true),
            GenreName = table.Column<string>(type: "TEXT", nullable: true)
          },
          constraints: table => {
            table.PrimaryKey("PK_Genre", x => x.Id);
            table.ForeignKey(
                      name: "FK_Genre_TitleAggs_TitleAggId",
                      column: x => x.TitleAggId,
                      principalTable: "TitleAggs",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Restrict);
          });

      migrationBuilder.CreateIndex(
          name: "IX_Genre_TitleAggId",
          table: "Genre",
          column: "TitleAggId");
    }

    protected override void Down(MigrationBuilder migrationBuilder) {
      migrationBuilder.DropTable(
          name: "Genre");

      migrationBuilder.DropTable(
          name: "TitleAggs");
    }
  }
}
