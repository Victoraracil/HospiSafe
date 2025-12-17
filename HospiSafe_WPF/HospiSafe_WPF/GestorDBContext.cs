using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HospiSafe_WPF
{
    class GestorDBContext : DbContext
    {
        //MODELOS
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<Cita> Citas { get; set; }
        public GestorDBContext() { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost,1433;Database=HospiSafe_BD;User Id=superadmin;Password=superpassword;TrustServerCertificate=True;"); //Configurar a futuro con docker sql dentro
        }
    }
}
