using Clase5.Data;
using Clase5.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Clase5.Controllers;

public class UsuarioController : Controller
{

    private readonly ApplicationDbContext _context;
    public UsuarioController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        if (User.Identity != null && User.Identity.IsAuthenticated)
        {
            var roleClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            switch (roleClaim)
            {
                case "Admin":
                    return RedirectToAction("Index", "Libros");

                case "User":
                    return RedirectToAction("Index", "Home");
            }
        }

        return View();
    }


    [HttpPost]
    public async Task<IActionResult> Login(UsuarioDto usuario)
    {

        var usuarioExiste = await ValidarUsuario(usuario);

        if (usuarioExiste == null)
        {
            return Unauthorized("Credenciales Incorrectas");
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name,usuarioExiste.Correo),
            new Claim(ClaimTypes.NameIdentifier,usuarioExiste.Id.ToString()),
            new Claim(ClaimTypes.Role,usuarioExiste.Role.Nombre)
        };

        var claimsAutenticadas = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsAutenticadas),
            new AuthenticationProperties
            {
                IsPersistent = false
            });

        switch (usuarioExiste.Role.Nombre)
        {
            case "Admin":
                return RedirectToAction("Index", "Libros");
            case "User":
                return RedirectToAction("Index", "Home");
        }

        return Ok();

    }

    private async Task<Usuario?> ValidarUsuario(UsuarioDto usuario)
    {

        var usuarioExistente = await _context.Usuario
                                    .Include(u => u.Role)
                                    .FirstOrDefaultAsync((u) => u.Correo.Equals(usuario.Correo));

        if (usuarioExistente == null)
        {
            return null;
        }

        var hash = ValidarHash(usuario.Contrasena, usuarioExistente.Salt);

        return usuarioExistente.Hash == hash ? usuarioExistente : null;

    }

    public string ValidarHash(string contrasena, string salt)
    {
        var combinada = contrasena + salt;
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(combinada));
        return Convert.ToBase64String(bytes);
    }

    public async Task<IActionResult> CrearUsuario()
    {
        var salt = GenerarSalt();

        var hash = GenerarHash("123456", salt);

        var usuario = new Usuario
        {
            Correo = "user@user.com",
            Hash = hash,
            Salt = salt,
            RoleId = 2
        };

        await _context.Usuario.AddAsync(usuario);
        await _context.SaveChangesAsync();

        return Ok("usuario Creado con exito");
    }

    public string GenerarSalt()
    {
        var bytes = new byte[16];

        using var rng = RandomNumberGenerator.Create();

        rng.GetBytes(bytes);

        return Convert.ToBase64String(bytes);

    }

    public string GenerarHash(string password, string salt)
    {
        using var sha256 = SHA256.Create();
        var combinada = password + salt;
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(combinada));
        return Convert.ToBase64String(bytes);
    }

    [HttpPost]
    public async Task<IActionResult> LogOut()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        return RedirectToAction("Index", "Usuario");
    }
}
