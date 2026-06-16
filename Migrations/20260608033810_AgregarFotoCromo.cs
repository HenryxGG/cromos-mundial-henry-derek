using Microsoft.EntityFrameworkCore.Migrations;

namespace cromosmundial_proyecto_final.Migrations
{
    public partial class AgregarFotoCromo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RutaFoto",
                table: "Cromos",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RutaFoto",
                table: "Cromos");
        }
    }
}