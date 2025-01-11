using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Security.Cryptography;

namespace RESTApi.Utility
{
    public static class PasswordManager
    {
        private static readonly string LowerCaseCharacterSet = "abcdefghijklmnopqrstuvxyz";

        private static readonly string UpperCaseCharacterSet = "ABCDEFGHIJKLMNOPQRSTUVXYZ";

        private static readonly string NumberCharacterSet = "0123456789";

        private static readonly string SpecialCharacterSet = "£$&()*+[]@#^-_!?";

        private const int CharacterCategories = 4;

        private const int AdminPasswordMinimumLength = 12;

        public static string HashPassword(string password)
        {
            // generating 128-bit salt using RNGCryptoServiceProvider
            byte[] salt = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }

            // using key derivation method to generate password hash
            string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8
            ));

            return $"{Convert.ToBase64String(salt)}:{hashedPassword}";
        }

        public static bool VerifyPassword(string hashedPassword, string password)
        {
            string[] parts = hashedPassword.Split(':');
            var salt = Convert.FromBase64String(parts[0]);
            string storedHash = parts[1];

            // deriving hash for entered password with stored hash's salt value
            string hashToVerify = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 32
            ));

            // comparing the hashToVerify and storedHash
            return storedHash == hashToVerify;
        }

        public static string GenerateAdminPassword()
        {
            string password = "";
            int index;
            Random randomNumberGenerator = new Random();
            for(int i=1;i<=AdminPasswordMinimumLength;i++)
            {
                switch(Math.Round(Math.Abs(randomNumberGenerator.NextDouble() * (CharacterCategories - 1))))
                {
                    case 0:
                        index = Convert.ToInt32(Math.Round(Math.Abs(randomNumberGenerator.NextDouble() * LowerCaseCharacterSet.Length - i)));
                        password += LowerCaseCharacterSet[index];
                        break;
                    case 1:
                        index = Convert.ToInt32(Math.Round(Math.Abs(randomNumberGenerator.NextDouble() * UpperCaseCharacterSet.Length - i)));
                        password += UpperCaseCharacterSet[index];
                        break;
                    case 2:
                        index = Convert.ToInt32(Math.Round(Math.Abs(randomNumberGenerator.NextDouble() * NumberCharacterSet.Length - i)));
                        password += NumberCharacterSet[index];
                        break;
                    case 3:
                        index = Convert.ToInt32(Math.Round(Math.Abs(randomNumberGenerator.NextDouble() * SpecialCharacterSet.Length - i)));
                        password += SpecialCharacterSet[index];
                        break;
                }
            }

            return password;
        }
    }
}