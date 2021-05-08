using BCrypt.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoEFTriggerProject.PasswordExtensions
{
    public static class GenerateAndVerifyPasswords
    {
        public static void CreatePasswordHash(this string password, out string passwordHash)
        {
            PassCheck(password);
            passwordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(password, hashType: HashType.SHA384);
        }

        public static bool VerifyPasswordHash(this string password, string passwordHash)
        {
            PassCheck(password);

            return BCrypt.Net.BCrypt.EnhancedVerify(password, passwordHash, hashType: HashType.SHA384);
        }

        private static void PassCheck(string password)
        {
            if (password == null)
                throw new ArgumentNullException(nameof(password), "Şifre alanı boş olamaz.");
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Şifrelerde boşluk kullanılamaz.", nameof(password));
        }
    }
}
