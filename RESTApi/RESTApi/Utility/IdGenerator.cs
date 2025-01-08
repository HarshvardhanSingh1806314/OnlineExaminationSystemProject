using System;
using System.Collections;
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

        public static string GenerateIdForStudent(string email, string phoneNo)
        {
            string input = $"{email}{phoneNo}{DateTime.Now}";
            return GenerateCustomId(input);
        }

        public static int GenerateIdForAdmin(ArrayList listOfExistingAdminIds)
        {
            Random adminIdGenerator = new Random();
            int adminId = adminIdGenerator.Next(100000, 999999);
            while(listOfExistingAdminIds.BinarySearch(adminId) >= 0)
            {
                adminId = adminIdGenerator.Next(100000, 999999);
            }

            return adminId;
        }

        public static string GenerateIdForTests(string name, string description)
        {
            string input = $"{name}{description}{DateTime.Now}";
            return GenerateCustomId(input);
        }

        public static string GenerateIdForQuestions(string description, string[] options, string answer)
        {
            string input = $"{description}";
            foreach(string option in options)
            {
                input += option;
            }
            return GenerateCustomId(input + answer + DateTime.Now.ToString());
        }

        public static string GenerateIdForReports(string studentId, string testId)
        {
            string input = $"{studentId}{testId}{DateTime.Now}";
            return GenerateCustomId(input);
        }
    }
}