namespace RESTApi.Utility
{
    public static class StaticDetails
    {
        public static string JWT_TOKEN_SECRET_KEY = null;

        public const string ROLE_STUDENT = "STUDENT";

        public const string ROLE_ADMIN = "ADMIN";

        public const string RESULT_PASSED = "PASSED";

        public const string RESULT_FAILED = "FAILED";

        public const string DIFFICULTY_EASY = "EASY";

        public const string DIFFICULTY_MEDIUM = "MEDIUM";

        public const string DIFFICULTY_HARD = "HARD";

        public const string ISSUER = "OnlineExaminationSystemBackend";

        public const string AUDIENCE = "OnlineExaminationSystemFrontend";

        public const string STUDENT_LOGIN_PATH = "/api/auth/student/login";

        public const string STUDENT_REGISTER_PATH = "/api/auth/student/register";

        public const string ADMIN_LOGIN_PATH = "/api/auth/admin/login";

        public const string ADMIN_REGISTER_PATH = "/api/auth/admin/register";

        public static string[] ADMIN_ROUTES =
        {
            "/api/test/add",
            "/api/test/remove",
            "/api/test/update",
            "/api/test/getalltests",
            "/api/test/getalltestandtestidlist",
            "/api/question/add",
            "/api/question/remove",
            "/api/question/update",
            "/api/question/getquestionsbytestid",
            "/api/question/getquestionsbydifficultylevel",
            "/api/question/getquestionbyid",
            "/api/report/getreportsbytestid",
            "/api/report/remove",
            "/api/report/update"
        };

        public static string[] STUDENT_ROUTES =
        {
            "/api/report/getallreports",
            "/api/test/gettestsbyorgname",
            "/api/test/gettestbyid",
            "/api/test/submittest"
        };

        public static string[] COMMON_ROUTES =
        {
            "/api/report/getreportbyid"
        };
    }
}