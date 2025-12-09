using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace biblioteca.Models
{
    public class Prestamo
    {
        [Key]
        public int ID_Prestamo { get; set; }

        [ForeignKey(nameof(Estudiante))]
        public string Registro_Estudiante { get; set; } = null!;

        [ForeignKey(nameof(Libro))]
        public int ID_Libro { get; set; }

        public DateTime FechaSalida { get; set; }
        public DateTime FechaLimite { get; set; }
        public DateTime? FechaDevolucion { get; set; }

        [ForeignKey(nameof(EstadoDevolucion))]
        public int? ID_Estado_Devolucion { get; set; }

        public Estudiante Estudiante { get; set; } = null!;
        public Libro Libro { get; set; } = null!;
        public EstadoLibro? EstadoDevolucion { get; set; }
    }
}
