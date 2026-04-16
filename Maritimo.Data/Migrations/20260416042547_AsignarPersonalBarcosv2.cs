using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Maritimo.Data.Migrations
{
    /// <inheritdoc />
    public partial class AsignarPersonalBarcosv2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PersonalBarcosRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonalId = table.Column<int>(type: "int", nullable: false),
                    BarcoId = table.Column<int>(type: "int", nullable: false),
                    RolId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalBarcosRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonalBarcosRoles_Barcos_BarcoId",
                        column: x => x.BarcoId,
                        principalTable: "Barcos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PersonalBarcosRoles_Personales_PersonalId",
                        column: x => x.PersonalId,
                        principalTable: "Personales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PersonalBarcosRoles_Roles_RolId",
                        column: x => x.RolId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PersonalBarcosRoles_BarcoId",
                table: "PersonalBarcosRoles",
                column: "BarcoId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalBarcosRoles_PersonalId",
                table: "PersonalBarcosRoles",
                column: "PersonalId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalBarcosRoles_RolId",
                table: "PersonalBarcosRoles",
                column: "RolId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PersonalBarcosRoles");
        }
    }
}
