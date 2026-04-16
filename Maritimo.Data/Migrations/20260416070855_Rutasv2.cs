using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Maritimo.Data.Migrations
{
    /// <inheritdoc />
    public partial class Rutasv2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Rutas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    puertoSalidaId = table.Column<int>(type: "int", nullable: false),
                    puertoLlegadaId = table.Column<int>(type: "int", nullable: false),
                    BarcoId = table.Column<int>(type: "int", nullable: false),
                    FechaPrevistaSalida = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaPrevistaLlegada = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rutas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rutas_Barcos_BarcoId",
                        column: x => x.BarcoId,
                        principalTable: "Barcos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Rutas_Puertos_puertoLlegadaId",
                        column: x => x.puertoLlegadaId,
                        principalTable: "Puertos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Rutas_Puertos_puertoSalidaId",
                        column: x => x.puertoSalidaId,
                        principalTable: "Puertos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rutas_BarcoId",
                table: "Rutas",
                column: "BarcoId");

            migrationBuilder.CreateIndex(
                name: "IX_Rutas_puertoLlegadaId",
                table: "Rutas",
                column: "puertoLlegadaId");

            migrationBuilder.CreateIndex(
                name: "IX_Rutas_puertoSalidaId",
                table: "Rutas",
                column: "puertoSalidaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rutas");
        }
    }
}
