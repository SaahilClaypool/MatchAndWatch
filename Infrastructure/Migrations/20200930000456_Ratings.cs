using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class Ratings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Rating",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    TitleId = table.Column<string>(type: "TEXT", nullable: true),
                    TitleId1 = table.Column<string>(type: "TEXT", nullable: true),
                    AverageRating = table.Column<float>(type: "REAL", nullable: false),
                    Votes = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rating", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rating_TitleAggs_TitleId",
                        column: x => x.TitleId,
                        principalTable: "TitleAggs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rating_TitleAggs_TitleId1",
                        column: x => x.TitleId1,
                        principalTable: "TitleAggs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rating_TitleId",
                table: "Rating",
                column: "TitleId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rating_TitleId1",
                table: "Rating",
                column: "TitleId1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rating");
        }
    }
}
