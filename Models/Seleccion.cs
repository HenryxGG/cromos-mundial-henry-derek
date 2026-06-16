using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace cromosmundial_proyecto_final.Models
{
    public class Seleccion
    {
        public int Id { get; set; }

        public required string Nombre { get; set; }
        [DisplayName("Confederación")]
        public required string Confederacion { get; set; }
        [DisplayName("Director Técnico")]
        public required string DirectorTecnico { get; set; }

        public ICollection<Cromo>? Cromos { get; set; }

        //Ruta imagen guardada en wwwroot/images

        [DisplayName("Foto de la Selección")]
        public string? RutaFoto { get; set; } // Propiedad para almacenar la ruta de la imagen guardada en el servidor

        //archivo de imagen subido por el usuario
        [NotMapped] // Esta propiedad no se mapea a la base de datos, solo se usa para manejar la carga de archivos en el formulario
        public IFormFile? ArchivoFoto { get; set; } // No se mapea a la base de datos, solo se usa para manejar la carga de archivos en el formulario
    }
}
