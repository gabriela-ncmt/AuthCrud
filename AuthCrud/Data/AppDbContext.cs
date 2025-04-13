using AuthCrud.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthCrud.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<UsuarioModel> Usuarios {get; set;}
        public DbSet<AuditoriaModel> Auditorias {get; set;}
    }
}
