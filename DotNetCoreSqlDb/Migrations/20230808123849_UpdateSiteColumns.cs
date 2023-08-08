using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotNetCoreSqlDb.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSiteColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Site");

            migrationBuilder.RenameColumn(
                name: "Priority",
                table: "Site",
                newName: "WANUplink");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Site",
                newName: "Status");

            migrationBuilder.AddColumn<int>(
                name: "AntalEnheter",
                table: "Site",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Epostadress",
                table: "Site",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GammalAdressEfterFlytt",
                table: "Site",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Gatuadress",
                table: "Site",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ISSKontorSite",
                table: "Site",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Kommentarer",
                table: "Site",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KontaktNamn",
                table: "Site",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Leverantör",
                table: "Site",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Mobilnr",
                table: "Site",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NätverkskapacitetGbps",
                table: "Site",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NätverkskapacitetMbps",
                table: "Site",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Ort",
                table: "Site",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SiteTyp",
                table: "Site",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Sitestorlek",
                table: "Site",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AntalEnheter",
                table: "Site");

            migrationBuilder.DropColumn(
                name: "Epostadress",
                table: "Site");

            migrationBuilder.DropColumn(
                name: "GammalAdressEfterFlytt",
                table: "Site");

            migrationBuilder.DropColumn(
                name: "Gatuadress",
                table: "Site");

            migrationBuilder.DropColumn(
                name: "ISSKontorSite",
                table: "Site");

            migrationBuilder.DropColumn(
                name: "Kommentarer",
                table: "Site");

            migrationBuilder.DropColumn(
                name: "KontaktNamn",
                table: "Site");

            migrationBuilder.DropColumn(
                name: "Leverantör",
                table: "Site");

            migrationBuilder.DropColumn(
                name: "Mobilnr",
                table: "Site");

            migrationBuilder.DropColumn(
                name: "NätverkskapacitetGbps",
                table: "Site");

            migrationBuilder.DropColumn(
                name: "NätverkskapacitetMbps",
                table: "Site");

            migrationBuilder.DropColumn(
                name: "Ort",
                table: "Site");

            migrationBuilder.DropColumn(
                name: "SiteTyp",
                table: "Site");

            migrationBuilder.DropColumn(
                name: "Sitestorlek",
                table: "Site");

            migrationBuilder.RenameColumn(
                name: "WANUplink",
                table: "Site",
                newName: "Priority");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Site",
                newName: "Description");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Site",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
