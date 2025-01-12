using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FrontEnd.Models
{
    public class ReportsData
    {
        public string StudentName { get; set; }

        public string TestName { get; set; }

        public int TestDuration { get; set; }

        public int CorrectAttemptsInEasyQuestions { get; set; }

        public int TotalAttemptsInEasyQuestions { get; set; }

        public int TotalNoOfEasyQuestions { get; set; }
        public int CorrectAttemptsInMediumQuestions { get; set; }

        public int TotalAttemptsInMediumQuestions { get; set; }

        public int TotalNoOfMediumQuestions { get; set; }
        public int CorrectAttemptsInHardQuestions { get; set; }

        public int TotalAttemptsInHardQuestions { get; set; }

        public int TotalNoOfHardQuestions { get; set; }
        public string Result { get; set; }


    }
}