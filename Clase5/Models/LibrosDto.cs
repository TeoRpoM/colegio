namespace Clase5.Models;

public class LibrosDto
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public IFormFile RutaPortada { get; set; }
    public IFormFile RutaLibro { get; set; }

}
