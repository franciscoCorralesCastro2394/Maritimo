using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Maritimo.Data.Migrations
{
    /// <inheritdoc />
    public partial class RutasCierre : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FechaCierre",
                table: "Rutas",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UsuarioCierre",
                table: "Rutas",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FechaCierre",
                table: "Rutas");

            migrationBuilder.DropColumn(
                name: "UsuarioCierre",
                table: "Rutas");
        }
    }
}
