using System;
using System.Security.Cryptography;
using System.Text;

namespace RESTApi.Utility
{
    public static class IdGenerator
    {
        private static string GenerateCustomId(string input)
        {
            using(var sha256 = SHA256.Create())
            {
                byte[] inputBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                return Convert.ToBase64String(inputBytes);
            }
        }
        public static string GenerateIdForDifficultyLevel(string difficultyLevel)
        {
            string input = $"{difficultyLevel.ToLower()}{DateTime.Now}";
            return GenerateCustomId(input);
        }
    }
}