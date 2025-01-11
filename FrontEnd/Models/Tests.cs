using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FrontEnd.Models
{
    public class Tests
    {
        public string TestId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int TotalNoOfEasyQuestions { get; set; }
        public int TotalNoOfMediumQuestions { get; set; } 
        public int TotalNoOfHardQuestions { get; set; }
        public int TotalNoOfQuestions { get; set; } 

        public int Duration { get; set; }
    }
}