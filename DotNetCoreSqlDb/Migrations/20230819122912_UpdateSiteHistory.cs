using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotNetCoreSqlDb.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSiteHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FieldName",
                table: "SiteHistories",
                newName: "PropertyName");

            migrationBuilder.RenameColumn(
                name: "ChangedDate",
                table: "SiteHistories",
                newName: "ChangedOn");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PropertyName",
                table: "SiteHistories",
                newName: "FieldName");

            migrationBuilder.RenameColumn(
                name: "ChangedOn",
                table: "SiteHistories",
                newName: "ChangedDate");
        }
    }
}
