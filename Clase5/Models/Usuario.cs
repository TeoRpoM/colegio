using System.Data;

namespace Clase5.Models;

public class Usuario
{

    public int Id { get; set; }
    public string Correo { get; set; }
    public string Hash { get; set; }
    public string Salt { get; set; }
    public int RoleId { get; set; }

    public Role Role { get; set; }

}
