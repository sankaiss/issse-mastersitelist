using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotNetCoreSqlDb.Migrations
{
    /// <inheritdoc />
    public partial class BilderFinal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SiteImage_Site_SiteID",
                table: "SiteImage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SiteImage",
                table: "SiteImage");

            migrationBuilder.RenameTable(
                name: "SiteImage",
                newName: "SiteImages");

            migrationBuilder.RenameIndex(
                name: "IX_SiteImage_SiteID",
                table: "SiteImages",
                newName: "IX_SiteImages_SiteID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SiteImages",
                table: "SiteImages",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_SiteImages_Site_SiteID",
                table: "SiteImages",
                column: "SiteID",
                principalTable: "Site",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SiteImages_Site_SiteID",
                table: "SiteImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SiteImages",
                table: "SiteImages");

            migrationBuilder.RenameTable(
                name: "SiteImages",
                newName: "SiteImage");

            migrationBuilder.RenameIndex(
                name: "IX_SiteImages_SiteID",
                table: "SiteImage",
                newName: "IX_SiteImage_SiteID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SiteImage",
                table: "SiteImage",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_SiteImage_Site_SiteID",
                table: "SiteImage",
                column: "SiteID",
                principalTable: "Site",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
