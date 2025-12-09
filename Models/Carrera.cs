using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace biblioteca.Models
{
    public class Carrera
    {
        [Key]
        public int ID_Carrera { get; set; }
        public string NombreCarrera { get; set; } = null!;

        public ICollection<Estudiante> Estudiantes { get; set; } = new List<Estudiante>();
    }
}
