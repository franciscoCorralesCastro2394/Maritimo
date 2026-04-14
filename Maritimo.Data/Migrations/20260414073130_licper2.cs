using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Maritimo.Data.Migrations
{
    /// <inheritdoc />
    public partial class licper2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LicenciasPersonal",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonalId = table.Column<int>(type: "int", nullable: false),
                    LicenciaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LicenciasPersonal", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LicenciasPersonal_LicenciasMaritimas_LicenciaId",
                        column: x => x.LicenciaId,
                        principalTable: "LicenciasMaritimas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LicenciasPersonal_Personales_PersonalId",
                        column: x => x.PersonalId,
                        principalTable: "Personales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LicenciasPersonal_LicenciaId",
                table: "LicenciasPersonal",
                column: "LicenciaId");

            migrationBuilder.CreateIndex(
                name: "IX_LicenciasPersonal_PersonalId",
                table: "LicenciasPersonal",
                column: "PersonalId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LicenciasPersonal");
        }
    }
}
