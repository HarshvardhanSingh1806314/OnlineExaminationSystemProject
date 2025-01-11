using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FrontEnd.Models
{
    public class Reports
    {
        public string Id { get; set; }
        public string StudentId { get; set; }
        public string TestId { get; set; }
        public int TotalAttemptsInEasyQuestions { get; set; }
        public int CorrectAttempsInEasyQuestions { get; set; }
        public int TotalAttemptsInMediumQuestions { get; set; } 
        public int CorrectAttemptsInMediumQuestions { get; set; } 
        public int TotalAttemptsInHardQuestions { get; set; }
        public int CorrectAttemptsInHardQuestions { get; set; }
    }
}