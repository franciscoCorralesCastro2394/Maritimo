using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Maritimo.Data.Migrations
{
    /// <inheritdoc />
    public partial class licenciasPersonalRelacion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LicenciasPersonal_LicenciasPersonal_LicenciaId",
                table: "LicenciasPersonal");

            migrationBuilder.AddForeignKey(
                name: "FK_LicenciasPersonal_LicenciasMaritimas_LicenciaId",
                table: "LicenciasPersonal",
                column: "LicenciaId",
                principalTable: "LicenciasMaritimas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LicenciasPersonal_LicenciasMaritimas_LicenciaId",
                table: "LicenciasPersonal");

            migrationBuilder.AddForeignKey(
                name: "FK_LicenciasPersonal_LicenciasPersonal_LicenciaId",
                table: "LicenciasPersonal",
                column: "LicenciaId",
                principalTable: "LicenciasPersonal",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
