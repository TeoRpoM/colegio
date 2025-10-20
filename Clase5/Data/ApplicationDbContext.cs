using Clase5.Models;
using Microsoft.EntityFrameworkCore;

namespace Clase5.Data;

public class ApplicationDbContext : DbContext
{

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options ) : base(options){}

    public DbSet<Estudiantes> Estudiantes { get; set; }
    public DbSet<Libros> Libros { get; set; }
    public DbSet<Usuario> Usuario { get; set; }
    public DbSet<Role> Role { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Usuario>()
            .HasOne(u => u.Role)
            .WithMany(r => r.Usuarios)
            .HasForeignKey(u => u.RoleId);

        base.OnModelCreating(modelBuilder);
    }

}
