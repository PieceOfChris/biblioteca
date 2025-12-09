using System.ComponentModel.DataAnnotations;

namespace biblioteca.Models
{
    public class Usuario
    {
        [Key]
        public int ID_Usuario { get; set; }
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string NombreCompleto { get; set; } = null!;
        public string Rol { get; set; } = null!; // "Administrador", "Bibliotecario", etc.
    }
}