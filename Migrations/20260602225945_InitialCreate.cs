using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cromosmundial_proyecto_final.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Jugadores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaNacimiento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Nacionalidad = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jugadores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Mundiales",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Anio = table.Column<int>(type: "int", nullable: false),
                    PaisSede = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mundiales", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Selecciones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Confederacion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DirectorTecnico = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Selecciones", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cromos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Codigo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumeroCamiseta = table.Column<int>(type: "int", nullable: false),
                    Posicion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JugadorId = table.Column<int>(type: "int", nullable: false),
                    SeleccionId = table.Column<int>(type: "int", nullable: false),
                    MundialId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cromos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cromos_Jugadores_JugadorId",
                        column: x => x.JugadorId,
                        principalTable: "Jugadores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Cromos_Mundiales_MundialId",
                        column: x => x.MundialId,
                        principalTable: "Mundiales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Cromos_Selecciones_SeleccionId",
                        column: x => x.SeleccionId,
                        principalTable: "Selecciones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cromos_JugadorId",
                table: "Cromos",
                column: "JugadorId");

            migrationBuilder.CreateIndex(
                name: "IX_Cromos_MundialId",
                table: "Cromos",
                column: "MundialId");

            migrationBuilder.CreateIndex(
                name: "IX_Cromos_SeleccionId",
                table: "Cromos",
                column: "SeleccionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cromos");

            migrationBuilder.DropTable(
                name: "Jugadores");

            migrationBuilder.DropTable(
                name: "Mundiales");

            migrationBuilder.DropTable(
                name: "Selecciones");
        }
    }
}
