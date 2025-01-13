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
            if (Request.Cookies.Get("ACCESS_TOKEN").Value != null)
            {
                Response.Cookies.Add(new HttpCookie("ACCESS_TOKEN")
                {
                    Expires = DateTime.Now.AddDays(-1)
                });
            }

            if(Request.Cookies.Get("ROLE").Value != null)
            {
                Response.Cookies.Add(new HttpCookie("ROLE") { 
                    Expires = DateTime.Now.AddDays(-1)
                });
            }
            ViewBag.Message = "Your Admin page.";
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(Admin admin)
        {
            if(ModelState.IsValid)
            {
                string accessToken = await RequestService.AdminLoginService(admin);
                string roleHash = IdGenerator.GenerateRoleId("ADMIN");
                StaticDetails.ROLE_ADMIN = roleHash;
                HttpCookie accessTokenCookie = new HttpCookie("ACCESS_TOKEN", accessToken)
                {
                    HttpOnly = true,
                    Secure = true,
                    Expires = DateTime.Now.AddDays(1)
                };
                Response.Cookies.Add(accessTokenCookie);
                HttpCookie roleCookie = new HttpCookie("ROLE", roleHash) { 
                    HttpOnly = true,
                    Expires = DateTime.Now.AddDays(1)
                };
                Response.Cookies.Add(roleCookie);
                return RedirectToAction(nameof(AdminPortal));
            }
            return View(admin);
        }
        public ActionResult AdminPortal()
        {
            //if (StaticDetails.ACTIVE_ROLE != null && StaticDetails.ACTIVE_ROLE.Equals("ADMIN"))
            //{
            //    return View();
            //}

            if(Request.Cookies.Get("ACCESS_TOKEN").Value != null && Request.Cookies.Get("ROLE").Value.Equals(StaticDetails.ROLE_ADMIN))
            {
                return View();
            }

            return RedirectToAction(nameof(Login));
        }
        public ActionResult AddTopic()
        {
            if(Request.Cookies.Get("ACCESS_TOKEN").Value != null && Request.Cookies.Get("ROLE").Value.Equals(StaticDetails.ROLE_ADMIN))
            {
                ViewBag.Message = "Your Add Topic page.";
                return View();
            }

            return RedirectToAction(nameof(Login));
        }
        [HttpPost]
        public async Task<ActionResult> AddTopic(AddTest test)
        {
            if (ModelState.IsValid)
            {
                string accessToken = Request.Cookies.Get("ACCESS_TOKEN").Value;
                Test newTest = await RequestService.CreateNewTest(test, accessToken);
                if(newTest != null)
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
        public ActionResult DeleteQuestion(Test t)
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
        public async Task<ActionResult> GetAllTopics()
        {
            string accessToken = Request.Cookies.Get("ACCESS_TOKEN").Value;
            if(accessToken != null && Request.Cookies.Get("ROLE").Value.Equals(StaticDetails.ROLE_ADMIN))
            {
                List<Test> testList = await RequestService.GetAllTests(accessToken);
                return View(testList);
            }

            return RedirectToAction(nameof(Login));
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


      




 