using HospiSafe.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospiSafe.Services
{
    public class ServicePaciente : IDisposable
    {
        bool disposed;
        public ServicePaciente()
        {
            disposed = false;
        }

        public async Task<List<Paciente>> ListarPacientesAsync()
        {
            using (var context = new GestorDBContext())
            {
                return await context.Pacientes
                    .AsNoTracking()
                    .OrderBy(p => p.IdPaciente)
                    .ToListAsync();
            }
        }

        public async Task<Paciente?> ObtenerPorIdAsync(int idPaciente)
        {
            using (var context = new GestorDBContext())
            {
                return await context.Pacientes
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p => p.IdPaciente == idPaciente);
            }
        }

        public async Task<bool> CrearPacienteAsync(Paciente paciente)
        {
            if (paciente == null)
                throw new ArgumentNullException(nameof(paciente));

            using (var context = new GestorDBContext())
            {
                bool existe = await context.Pacientes
                    .AnyAsync(p => p.DNI == paciente.DNI);

                if (existe)
                    return false;

                await context.Pacientes.AddAsync(paciente);
                await context.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> ActualizarPacienteAsync(Paciente paciente)
        {
            if (paciente == null)
                throw new ArgumentNullException(nameof(paciente));

            using (var context = new GestorDBContext())
            {
                var existente = await context.Pacientes
                    .FirstOrDefaultAsync(p => p.IdPaciente == paciente.IdPaciente);

                if (existente == null)
                    return false;

                existente.Nombre = paciente.Nombre;
                existente.Apellidos = paciente.Apellidos;
                existente.Telefono = paciente.Telefono;
                existente.CorreoElectronico = paciente.CorreoElectronico;
                existente.FechaNacimiento = paciente.FechaNacimiento;

                await context.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> EliminarPacienteAsync(int idPaciente)
        {
            using (var context = new GestorDBContext())
            {
                bool tieneCitas = await context.Citas
                    .AnyAsync(c => c.IdPaciente == idPaciente);

                if (tieneCitas)
                    return false;

                var paciente = await context.Pacientes.FindAsync(idPaciente);
                if (paciente == null)
                    return false;

                context.Pacientes.Remove(paciente);
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
            }

            disposed = true;
        }

        ~ServicePaciente()
        {
            Dispose(false);
        }
    }
}
