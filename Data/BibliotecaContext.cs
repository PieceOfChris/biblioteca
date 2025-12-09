using biblioteca.Models;
using Microsoft.EntityFrameworkCore;

namespace biblioteca.Data
{
    public class BibliotecaContext : DbContext
    {
        public BibliotecaContext(DbContextOptions<BibliotecaContext> options) : base(options) { }

        public DbSet<Autor> Autores { get; set; }
        public DbSet<Carrera> Carreras { get; set; }
        public DbSet<Editorial> Editoriales { get; set; }
        public DbSet<EstadoLibro> EstadosLibro { get; set; }
        public DbSet<Estudiante> Estudiantes { get; set; }
        public DbSet<Genero> Generos { get; set; }
        public DbSet<Libro> Libros { get; set; }
        public DbSet<LibroAutor> LibroAutores { get; set; }
        public DbSet<Prestamo> Prestamos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Clave compuesta Libro_Autores
            modelBuilder.Entity<LibroAutor>()
                .HasKey(la => new { la.ID_Libro, la.ID_Autor });

            // Relaciones muchos a muchos
            modelBuilder.Entity<LibroAutor>()
                .HasOne(la => la.Libro)
                .WithMany(l => l.LibroAutores)
                .HasForeignKey(la => la.ID_Libro);

            modelBuilder.Entity<LibroAutor>()
                .HasOne(la => la.Autor)
                .WithMany(a => a.LibroAutores)
                .HasForeignKey(la => la.ID_Autor);

            base.OnModelCreating(modelBuilder);
        }
    }
}

