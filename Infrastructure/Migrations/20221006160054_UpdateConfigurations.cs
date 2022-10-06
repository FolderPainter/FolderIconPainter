using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class UpdateConfigurations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomFolders_Categories_Id",
                table: "CustomFolders");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "CustomFolders",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<string>(
                name: "ColorHex",
                table: "CustomFolders",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_CustomFolders_CategoryId",
                table: "CustomFolders",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomFolders_Categories_CategoryId",
                table: "CustomFolders",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomFolders_Categories_CategoryId",
                table: "CustomFolders");

            migrationBuilder.DropIndex(
                name: "IX_CustomFolders_CategoryId",
                table: "CustomFolders");

            migrationBuilder.DropColumn(
                name: "ColorHex",
                table: "CustomFolders");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "CustomFolders",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomFolders_Categories_Id",
                table: "CustomFolders",
                column: "Id",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
