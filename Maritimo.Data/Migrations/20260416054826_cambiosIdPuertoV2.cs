using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Maritimo.Data.Migrations
{
    /// <inheritdoc />
    public partial class cambiosIdPuertoV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdPuerto",
                table: "Barcos");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdPuerto",
                table: "Barcos",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
