using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Maritimo.Data.Migrations
{
    /// <inheritdoc />
    public partial class BarcosYPuertos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Puertos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Puertos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Barcos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Matricula = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tonelaje = table.Column<int>(type: "int", nullable: false),
                    IdPuertoBase = table.Column<int>(type: "int", nullable: false),
                    PuertoId = table.Column<int>(type: "int", nullable: false),
                    ModeloMotor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PotenciaMotor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HoraUsoMotor = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Barcos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Barcos_Puertos_PuertoId",
                        column: x => x.PuertoId,
                        principalTable: "Puertos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Barcos_PuertoId",
                table: "Barcos",
                column: "PuertoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Barcos");

            migrationBuilder.DropTable(
                name: "Puertos");
        }
    }
}
