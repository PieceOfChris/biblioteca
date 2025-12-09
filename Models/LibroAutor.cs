using System.ComponentModel.DataAnnotations.Schema;

namespace biblioteca.Models
{
    public class LibroAutor
    {
        public int ID_Libro { get; set; }
        public int ID_Autor { get; set; }

        [ForeignKey(nameof(ID_Libro))]
        public Libro Libro { get; set; } = null!;

        [ForeignKey(nameof(ID_Autor))]
        public Autor Autor { get; set; } = null!;
    }
}