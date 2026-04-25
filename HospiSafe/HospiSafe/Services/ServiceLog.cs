using HospiSafe.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HospiSafe.Services
{
    public class ServiceLog : IDisposable
    {
        bool disposed;

        public ServiceLog()
        {
            disposed = false;
        }

        public async Task<List<Log>> ListarLogsAsync()
        {
            using var context = new GestorDBContext();
            return await context.Logs.AsNoTracking().OrderByDescending(l => l.Fecha).ToListAsync();
        }

        public async Task<bool> CrearLogAsync(Log log)
        {
            if (log == null) throw new ArgumentNullException(nameof(log));

            using var context = new GestorDBContext();
            await context.Logs.AddAsync(log);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CrearLogAsync(int? idUsuario, string accion)
        {
            var log = new Log
            {
                IdUsuario = idUsuario,
                Accion = accion ?? string.Empty
                // Fecha se asignara en la BD auto
            };
            return await CrearLogAsync(log);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed) return;
            disposed = true;
        }

        ~ServiceLog()
        {
            Dispose(false);
        }
    }
}