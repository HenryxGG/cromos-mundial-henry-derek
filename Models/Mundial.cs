using System.ComponentModel;

namespace cromosmundial_proyecto_final.Models
{
    public class Mundial
    {
        public int Id { get; set; }

        public required string Nombre { get; set; }

        public int Anio { get; set; }
        [DisplayName("País o Paises sedes")]
        public required string PaisSede { get; set; }

        public ICollection<Cromo>? Cromos { get; set; }
    }
}

