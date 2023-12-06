using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotNetCoreSqlDb.Migrations
{
    /// <inheritdoc />
    public partial class NewMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Site_SiteImages_SiteImageId1",
                table: "Site");

            migrationBuilder.DropTable(
                name: "SiteImages");

            migrationBuilder.DropIndex(
                name: "IX_Site_SiteImageId1",
                table: "Site");

            migrationBuilder.DropColumn(
                name: "SiteImageId",
                table: "Site");

            migrationBuilder.DropColumn(
                name: "SiteImageId1",
                table: "Site");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SiteImageId",
                table: "Site",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SiteImageId1",
                table: "Site",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "SiteImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SiteID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiteImages", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Site_SiteImageId1",
                table: "Site",
                column: "SiteImageId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Site_SiteImages_SiteImageId1",
                table: "Site",
                column: "SiteImageId1",
                principalTable: "SiteImages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
