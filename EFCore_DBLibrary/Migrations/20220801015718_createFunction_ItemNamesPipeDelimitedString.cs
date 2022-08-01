using Microsoft.EntityFrameworkCore.Migrations;
using EFCore_DBLibrary.Migrations.Scripts;
#nullable disable

namespace EFCore_DBLibrary.Migrations
{
    public partial class createFunction_ItemNamesPipeDelimitedString : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.SqlResource("EFCore_DBLibrary.Migrations.Scripts.Functions.ItemNamesPipeDelimitedString.ItemNamesPipeDelimitedString.v0.sql");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP FUNCTION IF EXISTS dbo.ItemNamesPipeDelimitedString");
        }
    }
}
