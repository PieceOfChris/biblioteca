// Program.cs (solo para probar, luego lo borran tus amigos del front)
using biblioteca.Data;
using biblioteca.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<BibliotecaContext>(opt =>
    opt.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=biblioteca;Trusted_Connection=true;"));

builder.Services.AddScoped<BibliotecaService>();

var app = builder.Build();

using var scope = app.Services.CreateScope();
var service = scope.ServiceProvider.GetRequiredService<BibliotecaService>();

// Ejemplos rápidos:
// var librosGabo = await service.BuscarLibrosPorAutorAsync("García Márquez");
// var morosos = await service.ListarMorososAsync();
// var popularidad = await service.ReportePopularidadGenerosAsync();

app.Run();
