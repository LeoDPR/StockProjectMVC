using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StockProject.Models;
namespace StockProject.Data;
public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    public DbSet<Products> Products { get; set; }
    public DbSet<Categorias> Categorias { get; set; }
    public DbSet<Asignacion> Asignacion { get; set; }
    public DbSet<Empleados> Empleados { get; set; }
}
