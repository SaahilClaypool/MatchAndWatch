using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class UpdateRating : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rating_TitleAggs_TitleId",
                table: "Rating");

            migrationBuilder.DropForeignKey(
                name: "FK_Rating_TitleAggs_TitleId1",
                table: "Rating");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Rating",
                table: "Rating");

            migrationBuilder.DropIndex(
                name: "IX_Rating_TitleId",
                table: "Rating");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Rating");

            migrationBuilder.RenameTable(
                name: "Rating",
                newName: "Ratings");

            migrationBuilder.RenameIndex(
                name: "IX_Rating_TitleId1",
                table: "Ratings",
                newName: "IX_Ratings_TitleId1");

            migrationBuilder.AlterColumn<string>(
                name: "TitleId",
                table: "Ratings",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ratings",
                table: "Ratings",
                column: "TitleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_TitleAggs_TitleId",
                table: "Ratings",
                column: "TitleId",
                principalTable: "TitleAggs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_TitleAggs_TitleId1",
                table: "Ratings",
                column: "TitleId1",
                principalTable: "TitleAggs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_TitleAggs_TitleId",
                table: "Ratings");

            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_TitleAggs_TitleId1",
                table: "Ratings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ratings",
                table: "Ratings");

            migrationBuilder.RenameTable(
                name: "Ratings",
                newName: "Rating");

            migrationBuilder.RenameIndex(
                name: "IX_Ratings_TitleId1",
                table: "Rating",
                newName: "IX_Rating_TitleId1");

            migrationBuilder.AlterColumn<string>(
                name: "TitleId",
                table: "Rating",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "Rating",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Rating",
                table: "Rating",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Rating_TitleId",
                table: "Rating",
                column: "TitleId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Rating_TitleAggs_TitleId",
                table: "Rating",
                column: "TitleId",
                principalTable: "TitleAggs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rating_TitleAggs_TitleId1",
                table: "Rating",
                column: "TitleId1",
                principalTable: "TitleAggs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
