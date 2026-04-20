using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Maritimo.Data.Migrations
{
    /// <inheritdoc />
    public partial class Ordenes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrdenesServicio",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUnico = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BarcoId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BarcoId1 = table.Column<int>(type: "int", nullable: false),
                    TipoManteniminto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Prioridad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descipcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fechalimite = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InformeCierre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaCierre = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsuarioCreador = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UsuarioCierre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrdenesServicio", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrdenesServicio_Barcos_BarcoId1",
                        column: x => x.BarcoId1,
                        principalTable: "Barcos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrdenesServicio_BarcoId1",
                table: "OrdenesServicio",
                column: "BarcoId1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrdenesServicio");
        }
    }
}
