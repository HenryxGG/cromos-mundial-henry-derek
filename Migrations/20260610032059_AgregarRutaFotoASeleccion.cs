using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cromosmundial_proyecto_final.Migrations
{
    /// <inheritdoc />
    public partial class AgregarRutaFotoASeleccion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RutaFoto",
                table: "Selecciones",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RutaFoto",
                table: "Selecciones");
        }
    }
}
