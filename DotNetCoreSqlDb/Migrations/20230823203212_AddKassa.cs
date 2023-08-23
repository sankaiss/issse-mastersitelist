using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotNetCoreSqlDb.Migrations
{
    /// <inheritdoc />
    public partial class AddKassa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Kassas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KontorSite = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gatuadress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ÄrendeNrISS = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ort = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ISSKontaktperson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EpostISSKontaktperson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TelefonISSKontaktperson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KassaTyp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Uppkopling = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Leveraantör = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Övrigt = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kassas", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Kassas");
        }
    }
}
