using cromosmundial_proyecto_final.Models.Validation;
using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace cromosmundial_proyecto_final.Models
{
    public class Cromo 
    {
        public int Id { get; set; }
        [DisplayName("Número de Camiseta")]
        public int NumeroCamiseta { get; set; }

        public required string Posicion { get; set; }
        [DisplayName("Jugador")]
        public int JugadorId { get; set; }
        public Jugador? Jugador { get; set; }
        [DisplayName("Selección")]
        public int SeleccionId { get; set; }
        public Seleccion? Seleccion { get; set; }
        [DisplayName("Mundial")]
        public int MundialId { get; set; }
        public Mundial? Mundial { get; set; }

        //Ruta imagen guardada en wwwroot/images
        public string? RutaFoto { get; set; }

        //archivo de imagen subido por el usuario
        [NotMapped]
        public IFormFile? ArchivoFoto { get; set; }
    }
}
