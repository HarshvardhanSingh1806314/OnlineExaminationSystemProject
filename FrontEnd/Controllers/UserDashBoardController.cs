using FrontEnd.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace FrontEnd.Controllers
{
    public class UserDashBoardController : Controller
    {
        // GET: UserDashBoard
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult StudentReportsuser()
        {
            return View();
        }

        public ActionResult ExamDisplay()
        {
            List<Questions> QuestionsList = new List<Questions>()
            {
                new Questions{

                    Description= "Which of the following statements is used to handle exceptions in C#?",
                    Option1= "catch",
                    Option2= "throw",
                    Option3= "try-catch",
                    Option4= "finally",
                    DifficultyLevel = "MEDIUM" },

                new Questions
                {
                       Description= "Which keyword is used to declare a constant in C#?",
                       Option1= "static",
                       Option2= "readonly",
                       Option3= "const",
                       Option4= "final",
                       DifficultyLevel= "MEDIUM",
                },
            };

            return View(QuestionsList);

        }
        public ActionResult ShowResult()
        {

            return View();

        }
    }
}