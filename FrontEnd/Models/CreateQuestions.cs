using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FrontEnd.Models
{
    public class CreateQuestions
    {
        public List<Questions> QuestionList { get; set; }

        public string TestId { get; set; }
    }
}