using HospiSafe.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospiSafe.Services
{
    public class ServiceInforme : IDisposable
    {
        bool disposed;
        public ServiceInforme()
        {
            disposed = false;
        }

        public async Task<List<Informe>> ListarInformesAsync()
        {
            using (var context = new GestorDBContext())
            {
                return await context.Informes
                    .Include(i => i.Prueba)
                    .Include(i => i.Paciente)
                    .AsNoTracking()
                    .OrderByDescending(i => i.Fecha)
                    .ToListAsync();
            }
        }

        public async Task<Informe?> ObtenerPorIdAsync(int idInforme)
        {
            using (var context = new GestorDBContext())
            {
                return await context.Informes
                    .Include(i => i.Prueba)
                    .Include(i => i.Paciente)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(i => i.IdInforme == idInforme);
            }
        }

        public async Task<bool> CrearInformeAsync(Informe informe)
        {
            if (informe == null)
                throw new ArgumentNullException(nameof(informe));

            using (var context = new GestorDBContext())
            {
                var prueba = await context.Pruebas.FindAsync(informe.IdPrueba);
                if (prueba == null) return false;

                // comprobar que no exista ya informe para esa prueba
                bool yaTiene = await context.Informes.AnyAsync(i => i.IdPrueba == informe.IdPrueba);
                if (yaTiene) return false;

                // asegurar paciente-prueba
                if (prueba.IdPaciente != informe.IdPaciente) return false;

                await context.Informes.AddAsync(informe);
                await context.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> ActualizarInformeAsync(Informe informe)
        {
            if (informe == null)
                throw new ArgumentNullException(nameof(informe));

            using (var context = new GestorDBContext())
            {
                var existente = await context.Informes
                    .FirstOrDefaultAsync(i => i.IdInforme == informe.IdInforme);

                if (existente == null) return false;

                existente.Contenido = informe.Contenido;
                existente.Fecha = informe.Fecha;

                await context.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> EliminarInformeAsync(int idInforme)
        {
            using (var context = new GestorDBContext())
            {
                var inf = await context.Informes.FindAsync(idInforme);
                if (inf == null) return false;
                context.Informes.Remove(inf);
                await context.SaveChangesAsync();
                return true;
            }
        }

        public async Task<List<Informe>> BuscarInformesAsync(string buscador)
        {
            using (var context = new GestorDBContext())
            {
                buscador = (buscador ?? string.Empty).Trim();

                if (string.IsNullOrEmpty(buscador))
                    return await ListarInformesAsync();

                return await context.Informes
                    .Include(i => i.Prueba)
                    .Include(i => i.Paciente)
                    .Where(i =>
                        i.Paciente.NumSS.Contains(buscador) ||
                        i.Paciente.DNI.Contains(buscador) ||
                        i.Paciente.Nombre.Contains(buscador) ||
                        i.Paciente.Apellidos.Contains(buscador)
                    )
                    .AsNoTracking()
                    .OrderByDescending(i => i.Fecha)
                    .ToListAsync();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;
            disposed = true;
        }

        ~ServiceInforme()
        {
            Dispose(false);
        }
    }
}
