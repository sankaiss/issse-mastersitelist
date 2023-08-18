using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotNetCoreSqlDb.Migrations
{
    /// <inheritdoc />
    public partial class RemovePostNr : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Postnr",
                table: "Etableringar",
                newName: "ÖvrigInfo");

            migrationBuilder.RenameColumn(
                name: "OvrigInfo",
                table: "Etableringar",
                newName: "ÄrendeNrISS");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ÖvrigInfo",
                table: "Etableringar",
                newName: "Postnr");

            migrationBuilder.RenameColumn(
                name: "ÄrendeNrISS",
                table: "Etableringar",
                newName: "OvrigInfo");
        }
    }
}
