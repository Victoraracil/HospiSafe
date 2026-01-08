using Microsoft.EntityFrameworkCore;

namespace HospiSafe_WPF.Models
{
    class GestorDBContext : DbContext
    {
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<Cita> Citas { get; set; }
        public GestorDBContext() { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost,1433;Database=HospiSafe_BD;User Id=superadmin;Password=superpassword;TrustServerCertificate=True;");
        }
    }
}
