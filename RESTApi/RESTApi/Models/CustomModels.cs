using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RESTApi.Models
{
    public class CustomModels
    {
        public struct TestUpdateModel
        {
            public string Name { get; set; }

            public string Description { get; set; }
        }

        public struct QuestionUpdateModel
        {
            public string Description { get; set; }

            public string Option1 { get; set; }

            public string Option2 { get; set; }

            public string Option3 { get; set; }

            public string Option4 { get; set; }

            public string Answer { get; set; }
        }
    }
}