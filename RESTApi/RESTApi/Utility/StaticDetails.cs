using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RESTApi.Utility
{
    public static class StaticDetails
    {
        public static string JWT_TOKEN_SECRET_KEY = null;

        public static string ROLE_STUDENT = "STUDENT";

        public static string ROLE_ADMIN = "ADMIN";

        public static string RESULT_PASSED = "PASSED";

        public static string RESULT_FAILED = "FAILED";

        public static string DIFFICULTY_EASY = "EASY";

        public static string DIFFICULTY_MEDIUM = "MEDIUM";

        public static string DIFFICULTY_HARD = "HARD";
    }
}