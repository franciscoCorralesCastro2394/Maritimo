using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Maritimo.Data.Migrations
{
    /// <inheritdoc />
    public partial class cambiosIdPuerto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IdPuertoBase",
                table: "Barcos",
                newName: "IdPuerto");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IdPuerto",
                table: "Barcos",
                newName: "IdPuertoBase");
        }
    }
}
