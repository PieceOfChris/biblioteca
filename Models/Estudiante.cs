using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace biblioteca.Models
{
    public class Estudiante
    {
        [Key]
        public string Registro { get; set; } = null!;
        public string Nombre { get; set; } = null!;
        public string ApellidoPaterno { get; set; } = null!;
        public string? ApellidoMaterno { get; set; }
        public string? Telefono { get; set; }

        [ForeignKey(nameof(Carrera))]
        public int ID_Carrera { get; set; }

        public Carrera Carrera { get; set; } = null!;
        public ICollection<Prestamo> Prestamos { get; set; } = new List<Prestamo>();
    }
}