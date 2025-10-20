using Clase5.Data;
using Clase5.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace Clase5.Controllers;

[Authorize(Roles = "Admin")]
public class LibrosController : Controller
{

    private readonly ApplicationDbContext _context;
    public LibrosController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> ListaDeLibros()
    {

        var listaLibros = await _context.Libros.ToListAsync();

        return Ok(listaLibros);

    }

    [HttpPost]
    public async Task<IActionResult> CrearLibro([FromForm] LibrosDto libro)
    {

        var rutaPortada = GuardarArchivo(libro.RutaPortada);
        var rutaLibro = GuardarArchivo(libro.RutaLibro);

        var entidad = new Libros
        {
            Nombre = libro.Nombre,
            RutaPortada = rutaPortada,
            RutaLibro = rutaLibro
        };

        await _context.Libros.AddAsync(entidad);

        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> ActualizarLibro([FromForm] LibrosDto libro)
    {

        var existente = await _context.Libros.FindAsync(libro.Id);

        existente.Nombre = libro.Nombre;

        EliminarLibro(existente.RutaLibro);
        existente.RutaLibro = GuardarArchivo(libro.RutaLibro);

        EliminarLibro(existente.RutaPortada);
        existente.RutaPortada = GuardarArchivo(libro.RutaPortada);

        _context.Libros.Update(existente);

        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> BorrarLibro(int id)
    {

        var existente = await _context.Libros.FindAsync(id);

        EliminarLibro(existente.RutaLibro);
        EliminarLibro(existente.RutaPortada);

        _context.Libros.Remove(existente);

        await _context.SaveChangesAsync();

        return Ok();

    }

    public string GuardarArchivo(IFormFile archivo)
    {

        var carpeta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "archivos");

        if (!Directory.Exists(carpeta))
        {
            Directory.CreateDirectory(carpeta);
        }

        var nombreArchivo = Guid.NewGuid() + Path.GetExtension(archivo.FileName);

        var rutaArchivo = Path.Combine(carpeta, nombreArchivo);

        using (var linea = new FileStream(rutaArchivo, FileMode.Create))
        {
            archivo.CopyTo(linea);
        }

        return "/archivos/" + nombreArchivo;
    }

    public void EliminarLibro(string ruta)
    {

        ruta = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot",ruta.TrimStart('/'));

        if (System.IO.File.Exists(ruta))
        {
            System.IO.File.Delete(ruta);
        }

    }

}
