using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotNetCoreSqlDb.Migrations
{
    /// <inheritdoc />
    public partial class KassaTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KontorSite",
                table: "Kassas");

            migrationBuilder.RenameColumn(
                name: "ÄrendeNrISS",
                table: "Kassas",
                newName: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Kassas",
                newName: "ÄrendeNrISS");

            migrationBuilder.AddColumn<string>(
                name: "KontorSite",
                table: "Kassas",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
