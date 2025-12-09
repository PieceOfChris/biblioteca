using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace biblioteca.Models
{
    public class Autor
    {
        public int ID_Autor { get; set; }
        public string NombreCompleto { get; set; } = null!;

        public ICollection<LibroAutor> LibroAutores { get; set; } = new List<LibroAutor>();
    }
}
