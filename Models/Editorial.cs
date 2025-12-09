using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace biblioteca.Models
{
    public class Editorial
    {
        [Key]
        public int ID_Editorial { get; set; }
        public string NombreEditorial { get; set; } = null!;

        public ICollection<Libro> Libros { get; set; } = new List<Libro>();
    }
}
