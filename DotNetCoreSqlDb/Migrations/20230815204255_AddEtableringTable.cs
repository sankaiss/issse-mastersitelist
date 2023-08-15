using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotNetCoreSqlDb.Migrations
{
    /// <inheritdoc />
    public partial class AddEtableringTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Etableringar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KontorSite = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AteaKonsult = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gatuadress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Postnr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ort = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ISSKontaktperson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EpostISSKontaktperson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TelefonISSKontaktperson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TeliaUppkoppling = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OvrigInfo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KlartSenast = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Etableringar", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Etableringar");
        }
    }
}
