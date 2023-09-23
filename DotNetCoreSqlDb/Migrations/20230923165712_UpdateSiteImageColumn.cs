using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotNetCoreSqlDb.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSiteImageColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SiteImages_Site_SiteID",
                table: "SiteImages");

            migrationBuilder.RenameColumn(
                name: "SiteId",
                table: "SiteImages",
                newName: "SiteID");

            migrationBuilder.RenameColumn(
                name: "SiteID",
                table: "SiteImages",
                newName: "SiteID1");

            migrationBuilder.RenameIndex(
                name: "IX_SiteImages_SiteID",
                table: "SiteImages",
                newName: "IX_SiteImages_SiteID1");

            migrationBuilder.AddForeignKey(
                name: "FK_SiteImages_Site_SiteID1",
                table: "SiteImages",
                column: "SiteID1",
                principalTable: "Site",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SiteImages_Site_SiteID1",
                table: "SiteImages");

            migrationBuilder.RenameColumn(
                name: "SiteID",
                table: "SiteImages",
                newName: "SiteId");

            migrationBuilder.RenameColumn(
                name: "SiteID1",
                table: "SiteImages",
                newName: "SiteID");

            migrationBuilder.RenameIndex(
                name: "IX_SiteImages_SiteID1",
                table: "SiteImages",
                newName: "IX_SiteImages_SiteID");

            migrationBuilder.AddForeignKey(
                name: "FK_SiteImages_Site_SiteID",
                table: "SiteImages",
                column: "SiteID",
                principalTable: "Site",
                principalColumn: "ID");
        }
    }
}
