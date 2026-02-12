using HospiSafe.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospiSafe.Services
{
    public class ServiceCita : IDisposable
    {
        bool disposed;

        public ServiceCita()
        {
            disposed = false;
        }

        public async Task<List<Cita>> ListarCitasAsync()
        {
            using (var context = new GestorDBContext())
            {
                return await context.Citas
                    .Include(c => c.Paciente)
                    .Include(c => c.Usuario)
                    .AsNoTracking()
                    .OrderBy(c => c.Fecha)
                    .ToListAsync();
            }
        }

        public async Task<bool> CrearCitaAsync(Cita cita)
        {
            if (cita == null)
                throw new ArgumentNullException(nameof(cita));

            using (var context = new GestorDBContext())
            {
                // Validate if user and patient exist? 
                // For now, assume they are selected from valid lists.
                await context.Citas.AddAsync(cita);
                await context.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> ActualizarCitaAsync(Cita cita)
        {
            if (cita == null)
                throw new ArgumentNullException(nameof(cita));

            using (var context = new GestorDBContext())
            {
                var existente = await context.Citas.FindAsync(cita.IdCita);

                if (existente == null)
                    return false;

                existente.Fecha = cita.Fecha;
                existente.Estado = cita.Estado;
                existente.IdPaciente = cita.IdPaciente;
                existente.IdUsuario = cita.IdUsuario;

                await context.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> EliminarCitaAsync(int idCita)
        {
            using (var context = new GestorDBContext())
            {
                var cita = await context.Citas.FindAsync(idCita);
                if (cita == null)
                    return false;

                context.Citas.Remove(cita);
                await context.SaveChangesAsync();
                return true;
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

            if (disposing)
            {
                // Free managed resources if needed
            }

            disposed = true;
        }

        ~ServiceCita()
        {
            Dispose(false);
        }
    }
}
