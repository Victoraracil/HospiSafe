using System;
using System.Security.Cryptography;
using System.Text;

namespace HospiSafe_WPF.Services
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
                    byte[] bytes = Encoding.UTF8.GetBytes(password);
                    byte[] hash = sha.ComputeHash(bytes);
                    return Convert.ToHexString(hash);
                }
            }
        }
    }
}
