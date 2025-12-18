using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//Para el SHA256
using System.Security.Cryptography;


///<author> Ruben Gumpert </author>

namespace GestionTareas
{
    public static class PasswordHelper
    {
        public static string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return string.Empty;
            }
            else
            {
                using (SHA256 sha = SHA256.Create())
                {
                    // hash trabajan con bytes, pasamos password a bytes
                    byte[] bytes = Encoding.UTF8.GetBytes(password);
                    // array de byes con la contraseña hasheadaX
                    byte[] hash = sha.ComputeHash(bytes);
                    // Devuelve el hash en formato hexadecimal
                    return Convert.ToHexString(hash);
                }
            }
        }
    }
}
