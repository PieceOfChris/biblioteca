using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace biblioteca.Models
{
    public class Libro
    {
        [Key]
        public int ID_Libro { get; set; }
        public string Titulo { get; set; } = null!;
        public int? AnioPublicacion { get; set; }
        public int NumeroEjemplares { get; set; }
        public int EjemplaresDisponibles { get; set; }

        [ForeignKey(nameof(Editorial))]
        public int ID_Editorial { get; set; }

        [ForeignKey(nameof(Genero))]
        public int ID_Genero { get; set; }

        public Editorial Editorial { get; set; } = null!;
        public Genero Genero { get; set; } = null!;
        public ICollection<LibroAutor> LibroAutores { get; set; } = new List<LibroAutor>();
        public ICollection<Prestamo> Prestamos { get; set; } = new List<Prestamo>();
    }
}