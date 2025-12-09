using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace biblioteca.Models
{
    public class Genero
    {
        [Key]
        public int ID_Genero { get; set; }
        public string NombreGenero { get; set; } = null!;

        public ICollection<Libro> Libros { get; set; } = new List<Libro>();
    }
}
