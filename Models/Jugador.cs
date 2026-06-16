using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using cromosmundial_proyecto_final.Models.Validation;

namespace cromosmundial_proyecto_final.Models
{
    public class Jugador
    {
        public int Id { get; set; }

        public required string Nombre { get; set; }
        [DisplayName("Fecha de Nacimiento")]
        [NoFuturo(ErrorMessage = "La fecha de nacimiento incorrecta...")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime FechaNacimiento { get; set; }


        public required string Nacionalidad { get; set; }

        public ICollection<Cromo>? Cromos { get; set; }

        [DisplayName("Historia del Jugador")]
        [DataType(DataType.MultilineText)] // Esto ayuda a que en las vistas se genere un cuadro de texto grande
        public string? Historia { get; set; }
    }
}
