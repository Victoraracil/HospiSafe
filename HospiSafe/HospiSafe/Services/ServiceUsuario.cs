using HospiSafe.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospiSafe.Services
{
    public class ServiceUsuario : IDisposable
    {
        bool disposed;
        public ServiceUsuario()
        {
            disposed = false;
        }

        public async Task<Usuario?> LoginAsync(string correo, string passwordIntroducida)
        {
            using (var context = new GestorDBContext())
            {
                var usuario = await context.Usuarios
                    .FirstOrDefaultAsync(u => u.CorreoElectronico == correo);

                if (usuario == null)
                    return null;

                string passwordHashIn = PasswordHelper.HashPassword(passwordIntroducida);

                if (passwordHashIn != usuario.PasswordHash)
                    return null;

                return usuario;
            }
        }

        public async Task<List<Usuario>> ListarUsuariosAsync()
        {
            using (var context = new GestorDBContext())
            {
                return await context.Usuarios
                    .AsNoTracking()
                    .OrderBy(u => u.IdUsuario)
                    .ToListAsync();
            }
        }

        public async Task<bool> CrearUsuarioAsync(Usuario usuario)
        {
            if (usuario == null)
                throw new ArgumentNullException(nameof(usuario));

            using (var context = new GestorDBContext())
            {
                bool existe = await context.Usuarios
                    .AnyAsync(u => u.CorreoElectronico == usuario.CorreoElectronico);

                if (existe)
                    return false;

                usuario.PasswordHash = PasswordHelper
                    .HashPassword(usuario.PasswordHash);

                await context.Usuarios.AddAsync(usuario);
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

        ~ServiceUsuario()
        {
            Dispose(false);
        }
    }
}
