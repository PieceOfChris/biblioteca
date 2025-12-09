using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace biblioteca.Models
{
    public class EstadoLibro
    {
        [Key]
        public int ID_Estado { get; set; }
        public string DescripcionEstado { get; set; } = null!;

        public ICollection<Prestamo> Prestamos { get; set; } = new List<Prestamo>();
    }
}
