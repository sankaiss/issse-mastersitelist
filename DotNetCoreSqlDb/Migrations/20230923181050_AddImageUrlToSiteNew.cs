using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotNetCoreSqlDb.Migrations
{
    /// <inheritdoc />
    public partial class AddImageUrlToSiteNew : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SiteImages");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Site",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Site");

            migrationBuilder.CreateTable(
                name: "SiteImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SiteID1 = table.Column<int>(type: "int", nullable: true),
                    SiteID = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiteImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SiteImages_Site_SiteID1",
                        column: x => x.SiteID1,
                        principalTable: "Site",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SiteImages_SiteID1",
                table: "SiteImages",
                column: "SiteID1");
        }
    }
}
