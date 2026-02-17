using HospiSafe.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Threading.Tasks;

namespace HospiSafe.Services
{
    public class ServicePrueba : IDisposable
    {
        bool disposed;

        public ServicePrueba()
        {
            disposed = false;
        }

        public async Task<List<Prueba>> ListarPruebasAsync()
        {
            using (var context = new GestorDBContext())
            {
                return await context.Pruebas
                    .AsNoTracking()
                    .Include(p => p.Paciente)
                    .Include(p => p.Usuario)
                    .OrderByDescending(p => p.Fecha)
                    .ToListAsync();
            }
        }

        public async Task<bool> CrearPruebaAsync(Prueba prueba)
        {
            if (prueba == null)
                throw new ArgumentNullException(nameof(prueba));

            using (var context = new GestorDBContext())
            {
                await context.Pruebas.AddAsync(prueba);
                await context.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> ActualizarPruebaAsync(Prueba prueba)
        {
            if (prueba == null)
                throw new ArgumentNullException(nameof(prueba));

            using (var context = new GestorDBContext())
            {
                var existente = await context.Pruebas
                    .FirstOrDefaultAsync(p => p.IdPrueba == prueba.IdPrueba);

                if (existente == null)
                    return false;

                existente.Fecha = prueba.Fecha;
                existente.TipoAnalisis = prueba.TipoAnalisis;
                existente.Estado = prueba.Estado;
                existente.Resultados = prueba.Resultados;
                existente.IdPaciente = prueba.IdPaciente;
                existente.IdUsuario = prueba.IdUsuario;

                await context.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> EliminarPruebaAsync(int idPrueba)
        {
            using (var context = new GestorDBContext())
            {
                var prueba = await context.Pruebas.FindAsync(idPrueba);
                if (prueba == null)
                    return false;

                context.Pruebas.Remove(prueba);
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

            disposed = true;
        }

        ~ServicePrueba()
        {
            Dispose(false);
        }
    }

}
