using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace FrontEnd.Utility
{
    public static class IdGenerator
    {
        private static string GenerateCustomId(string input)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] inputBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                return Convert.ToBase64String(inputBytes);
            }
        }

        public static string GenerateRoleId(string role)
        {
            string input = $"{role}{DateTime.Now}";
            return GenerateCustomId(input);
        }
    }
}