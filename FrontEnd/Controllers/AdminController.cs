using FrontEnd.AsyncServices;
using FrontEnd.Models;
using FrontEnd.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;


namespace FrontEnd.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            Response.Cookies.Remove("ACCESS_TOKEN");
            ViewBag.Message = "Your Admin page.";
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(Admin admin)
        {
            if(ModelState.IsValid)
            {
                string accessToken = await RequestService.AdminLoginService(admin);
                string adminRoleHash = IdGenerator.GenerateRoleId("ADMIN");
                StaticDetails.ROLE_ADMIN = adminRoleHash;
                HttpCookie accessTokenCookie = new HttpCookie("ACCESS_TOKEN", accessToken)
                {
                    HttpOnly = true,
                    Secure = true,
                    Expires = DateTime.Now.AddDays(1)
                };
                HttpCookie roleCookie = new HttpCookie("ROLE", adminRoleHash)
                {
                    HttpOnly = true,
                    Expires = DateTime.Now.AddDays(1)
                };
                Response.Cookies.Add(accessTokenCookie);
                Response.Cookies.Add(roleCookie);
                return RedirectToAction(nameof(AdminPortal));
            }
            return View(admin);
        }
        public ActionResult AdminPortal()
        {
            return View();
        }
        public ActionResult AddTopic()
        {
            ViewBag.Message = "Your Add Topic page.";

            return View();
        }
        [HttpPost]
        public ActionResult AddTopic(Tests test)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction(nameof(GetAllTopics));
            }
            return View(test);
        }
        //public ActionResult AddQuestion()
        //{
        //    ViewBag.Message = "Your Add Question page.";

        //    return View();
        //}
        //[HttpPost]
        //public ActionResult AddQuestion(Questions question)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        return RedirectToAction(nameof(GetAllQuestions));
        //    }
        //    return View(question);
        //}
        public ActionResult DeleteQuestion()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DeleteQuestion(Tests t)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction(nameof(GetAllQuestions));
            }
            return View(t);
        }
        public ActionResult ViewReport()
        {
            var model = new TestDropDown{ 
                Items= new List<SelectListItem> {
                    new SelectListItem { Text = "CSharp", Value = "1" },
                    new SelectListItem { Text = "SQL", Value = "2" },
                    new SelectListItem { Text = "Dotnet", Value = "3" },
                    new SelectListItem { Text = "Java", Value = "4" },
                }};
            return View(model);
        }
        [HttpPost]
        public ActionResult ViewReport(string TestName)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction(nameof(StudentReports));
            }
            return View();

        }

        //get list of topics
        public ActionResult GetAllTopics()
        {
            //List<Tests> testlist = db.tests.ToList();
            //return View(testlist);
            return View();
        }
        public ActionResult GetAllQuestions()
        {
            List<Questions> QuestionsList = new List<Questions>()
            {
                new Questions{
                    Id="Xh2Cn/W9mPorzijPK9QTFGUCPn0SYumMBc29BzJ8ZtA=",
                    Description= "Which of the following statements is used to handle exceptions in C#?",
                    Option1= "catch",
                    Option2= "throw",
                    Option3= "try-catch",
                    Option4= "finally",
                    Answer="try-catch",
                    DifficultyLevel = "MEDIUM",
                    TestId="FYznnbjioGX1QA89p7eRxPZcF/bu5yOX0neIQ7jLwos="},
            };
            
            return View(QuestionsList);
        }
        public ActionResult DeleteQuestions(Questions q)
        {
            return View(q);
        }
        [HttpPost]
        public ActionResult DeleteQuestionspost(Questions q)
        {
            if(ModelState.IsValid)
            {
                return RedirectToAction(nameof(GetAllQuestions));
            }
            return View(q);
        }
        //edit question
        public ActionResult EditQuestions(Questions q)
        {
            return View(q);
        }
        [HttpPost]
        public ActionResult EditQuestionspost(Questions q)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction(nameof(GetAllQuestions));
            }
            return View(q);
        }

        public ActionResult CreateQuestions()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateQuestions(string TestId,Questions questions)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction(nameof(GetAllQuestions));
            }
            return View(questions);

        }

        public ActionResult StudentReports()
        {
            return View();
        }
    }
}


      




 