using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FrontEnd.Models
{
    public class Result
    {
        public string StudentName { get; set; }

        public string TestName { get; set; }

        public string DifficultyLevel { get; set; }

        public int CorrectAttempts { get; set; }

        public int TotalAttempts { get; set; }

        public int TotalQuestions { get; set; }

        public string TestResult { get; set; }

        public string ReportId { get; set; }
    }
}