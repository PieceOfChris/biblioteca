using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using biblioteca.Data;
using biblioteca.Models;
using Microsoft.EntityFrameworkCore;

namespace biblioteca.Services
{
    public class BibliotecaService
    {
        private readonly BibliotecaContext _context;

        public BibliotecaService(BibliotecaContext context)
        {
            _context = context;
        }

        // 1. Autenticar Bibliotecario
        public Usuario? Autenticar(string email, string passwordPlana)
        {
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Email == email);
            if (usuario == null) return null;

            // Aquí deberías usar BCrypt o similar, pero por simplicidad:
            return usuario.PasswordHash == HashPassword(passwordPlana) ? usuario : null;
        }

        private string HashPassword(string password) => Convert.ToHexString(System.Security.Cryptography.SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(password)));

        // 2. Catalogar Libro
        public async Task<Libro> CatalogarLibroAsync(string titulo, int? anio, int numEjemplares, int idEditorial, int idGenero, List<int> autoresIds)
        {
            var libro = new Libro
            {
                Titulo = titulo,
                AnioPublicacion = anio,
                NumeroEjemplares = numEjemplares,
                EjemplaresDisponibles = numEjemplares,
                ID_Editorial = idEditorial,
                ID_Genero = idGenero
            };

            _context.Libros.Add(libro);
            await _context.SaveChangesAsync();

            foreach (var idAutor in autoresIds)
            {
                _context.LibroAutores.Add(new LibroAutor { ID_Libro = libro.ID_Libro, ID_Autor = idAutor });
            }
            await _context.SaveChangesAsync();

            return libro;
        }

        // 3. Registrar Estudiante
        public async Task<Estudiante> RegistrarEstudianteAsync(string registro, string nombre, string apePaterno, string? apeMaterno, string? telefono, int idCarrera)
        {
            var est = new Estudiante
            {
                Registro = registro,
                Nombre = nombre,
                ApellidoPaterno = apePaterno,
                ApellidoMaterno = apeMaterno,
                Telefono = telefono,
                ID_Carrera = idCarrera
            };
            _context.Estudiantes.Add(est);
            await _context.SaveChangesAsync();
            return est;
        }

        // 4. Prestar Libro
        public async Task<bool> PrestarLibroAsync(string registro, int idLibro, int diasPrestamo = 14)
        {
            var libro = await _context.Libros.FindAsync(idLibro);
            if (libro == null || libro.EjemplaresDisponibles <= 0) return false;

            var prestamo = new Prestamo
            {
                Registro_Estudiante = registro,
                ID_Libro = idLibro,
                FechaSalida = DateTime.Today,
                FechaLimite = DateTime.Today.AddDays(diasPrestamo),
                ID_Estado_Devolucion = null
            };

            libro.EjemplaresDisponibles--;
            _context.Prestamos.Add(prestamo);
            await _context.SaveChangesAsync();
            return true;
        }

        // 5. Devolver Libro
        public async Task<bool> DevolverLibroAsync(int idPrestamo, int idEstado = 1) // 1 = Devuelto en buen estado
        {
            var prestamo = await _context.Prestamos.Include(p => p.Libro).FirstOrDefaultAsync(p => p.ID_Prestamo == idPrestamo);
            if (prestamo == null || prestamo.FechaDevolucion != null) return false;

            prestamo.FechaDevolucion = DateTime.Today;
            prestamo.ID_Estado_Devolucion = idEstado;
            prestamo.Libro.EjemplaresDisponibles++;

            await _context.SaveChangesAsync();
            return true;
        }

        // 6. Buscar por Autor (con JOIN LINQ obligatorio)
        public async Task<List<string>> BuscarLibrosPorAutorAsync(string nombreAutor)
        {
            var resultado = await (from libro in _context.Libros
                                   join la in _context.LibroAutores on libro.ID_Libro equals la.ID_Libro
                                   join autor in _context.Autores on la.ID_Autor equals autor.ID_Autor
                                   where autor.NombreCompleto.Contains(nombreAutor)
                                   select $"{libro.Titulo} - {autor.NombreCompleto}")
                                   .Distinct()
                                   .ToListAsync();

            return resultado;
        }

        // 7. Listar Morosos (WHERE LINQ obligatorio: préstamos vencidos)
        public async Task<List<object>> ListarMorososAsync()
        {
            var hoy = DateTime.Today;

            var morosos = await _context.Prestamos
                .Include(p => p.Estudiante)
                .Include(p => p.Libro)
                .Where(p => p.FechaDevolucion == null && p.FechaLimite < hoy)
                .Select(p => new
                {
                    Estudiante = $"{p.Estudiante.Nombre} {p.Estudiante.ApellidoPaterno}",
                    p.Estudiante.Registro,
                    Libro = p.Libro.Titulo,
                    p.FechaLimite,
                    DiasAtraso = (hoy - p.FechaLimite).Days
                })
                .ToListAsync();

            return morosos.Cast<object>().ToList();
        }

        // 8. Renovar Préstamo
        public async Task<bool> RenovarPrestamoAsync(int idPrestamo, int diasExtra = 7)
        {
            var prestamo = await _context.Prestamos.FindAsync(idPrestamo);
            if (prestamo == null || prestamo.FechaDevolucion != null) return false;

            prestamo.FechaLimite = prestamo.FechaLimite.AddDays(diasExtra);
            await _context.SaveChangesAsync();
            return true;
        }

        // 9. Baja de Libros (marcar como perdido/dañado)
        public async Task<bool> DarDeBajaLibroAsync(int idLibro, int nuevoEstado = 3) // supongamos 3=Perdido, 4=Dañado
        {
            var libro = await _context.Libros.FindAsync(idLibro);
            if (libro == null) return false;

            libro.NumeroEjemplares--;
            if (libro.EjemplaresDisponibles > libro.NumeroEjemplares)
                libro.EjemplaresDisponibles = libro.NumeroEjemplares;

            await _context.SaveChangesAsync();
            return true;
        }

        // 10. Reporte de Popularidad de Géneros (COUNT + GROUP BY)
        public async Task<List<object>> ReportePopularidadGenerosAsync()
        {
            var reporte = await _context.Prestamos
                .Include(p => p.Libro.Genero)
                .Where(p => p.FechaDevolucion != null) // solo préstamos completados
                .GroupBy(p => p.Libro.Genero.NombreGenero)
                .Select(g => new
                {
                    Genero = g.Key,
                    CantidadPrestamos = g.Count()
                })
                .OrderByDescending(x => x.CantidadPrestamos)
                .ToListAsync();

            return reporte.Cast<object>().ToList();
        }

        // Función extra pedida: cuántos libros tiene prestados un estudiante (COUNT LINQ obligatorio)
        public int CantidadLibrosPrestados(string registroEstudiante)
        {
            return _context.Prestamos
                .Count(p => p.Registro_Estudiante == registroEstudiante && p.FechaDevolucion == null);
        }
    }
}
