using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HospiSafe.Services
{
    public static class PasswordHelper
    {
        private const int SaltSize = 16;
        private const int Iterations = 100000; //num iteraciones
        private const int KeySize = 32; // 256 bits

        public static string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return string.Empty;
            }

            using (var numeroRandom = RandomNumberGenerator.Create())
            {
                byte[] salt = new byte[SaltSize]; //salt 128bits
                numeroRandom.GetBytes(salt); //extraemos bytes de salt

                using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256))
                {
                    byte[] hash = pbkdf2.GetBytes(KeySize);

                    // Combinamos salt + hash para almacenar juntos
                    byte[] hashWithSalt = new byte[salt.Length + hash.Length];
                    Buffer.BlockCopy(salt, 0, hashWithSalt, 0, salt.Length);
                    Buffer.BlockCopy(hash, 0, hashWithSalt, salt.Length, hash.Length);

                    return Convert.ToHexString(hashWithSalt);
                }
            }
        }

        public static bool VerifyPassword(string password, string hash)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(hash))
            {
                return false;
            }

            try
            {
                byte[] hashWithSalt = Convert.FromHexString(hash);
                byte[] salt = new byte[SaltSize];
                Buffer.BlockCopy(hashWithSalt, 0, salt, 0, SaltSize);

                using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256))
                {
                    byte[] hashToCompare = pbkdf2.GetBytes(KeySize);

                    // Comparar el hash generado con el almacenado
                    for (int i = 0; i < KeySize; i++)
                    {
                        if (hashToCompare[i] != hashWithSalt[SaltSize + i])
                            return false;
                    }

                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
