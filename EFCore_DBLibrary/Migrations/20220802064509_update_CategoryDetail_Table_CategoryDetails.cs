using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFCore_DBLibrary.Migrations
{
    public partial class update_CategoryDetail_Table_CategoryDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryDetail_Categories_Id",
                table: "CategoryDetail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CategoryDetail",
                table: "CategoryDetail");

            migrationBuilder.RenameTable(
                name: "CategoryDetail",
                newName: "CategoryDetails");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CategoryDetails",
                table: "CategoryDetails",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryDetails_Categories_Id",
                table: "CategoryDetails",
                column: "Id",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryDetails_Categories_Id",
                table: "CategoryDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CategoryDetails",
                table: "CategoryDetails");

            migrationBuilder.RenameTable(
                name: "CategoryDetails",
                newName: "CategoryDetail");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CategoryDetail",
                table: "CategoryDetail",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryDetail_Categories_Id",
                table: "CategoryDetail",
                column: "Id",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
