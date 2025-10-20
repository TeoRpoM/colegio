using Clase5.Data;
using Clase5.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Clase5.Controllers;

[Authorize(Roles = "Admin")]
public class EstudiantesController : Controller
{
    private readonly ApplicationDbContext _context;
    public EstudiantesController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> ListaDeEstudiantes()
    {

        var listaEstudiantes = await _context.Estudiantes.ToListAsync();

        return Ok(listaEstudiantes);
    }

    [HttpPost]
    public async Task<IActionResult> CrearEstudiante([FromBody] Estudiantes estudiante)
    {

        await _context.Estudiantes.AddAsync(estudiante);

        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> ActualizarEstudiante([FromBody] Estudiantes estudiante)
    {

        var existente = await _context.Estudiantes.FindAsync(estudiante.Id);

        existente.Nombre = estudiante.Nombre;
        existente.Edad = estudiante.Edad;
        existente.Grado = estudiante.Grado;

        _context.Estudiantes.Update(existente);

        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> BorrarEstudiante(int id)
    {
        try
        {
            var existente = await _context.Estudiantes.FindAsync(id);

            _context.Estudiantes.Remove(existente);

            await _context.SaveChangesAsync();

            return Ok();
        }
        catch (Exception ex)
        {
            return Ok();
        }

    }

}
