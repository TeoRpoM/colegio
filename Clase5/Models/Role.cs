namespace Clase5.Models;

public class Role
{
    public int Id { get; set; }
    public string Nombre { get; set; }

    public List<Usuario> Usuarios { get; set; }
}
