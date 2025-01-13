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
<<<<<<< HEAD
            var accessTokenCookie = Request.Cookies.Get("ACCESS_TOKEN");
            var roleCookie = Request.Cookies.Get("ROLE");
            if (accessTokenCookie != null && accessTokenCookie.Value != null)
=======
            if (Request.Cookies.Get("ACCESS_TOKEN") != null)
>>>>>>> 0dd44482edffc90630b2a0781dc87a7b744bdb28
            {
                Response.Cookies.Add(new HttpCookie("ACCESS_TOKEN")
                {
                    Expires = DateTime.Now.AddDays(-1)
                });
            }

<<<<<<< HEAD
            if (roleCookie != null && roleCookie.Value != null)
=======
            if(Request.Cookies.Get("ROLE") != null)
>>>>>>> 0dd44482edffc90630b2a0781dc87a7b744bdb28
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
            var accessTokenCookie = Request.Cookies.Get("ACCESS_TOKEN");
            var roleCookie = Request.Cookies.Get("ROLE");

            if (accessTokenCookie != null && 
                accessTokenCookie.Value != null &&
                roleCookie != null &&
                roleCookie.Value.Equals(StaticDetails.ROLE_ADMIN)
            )
            {
                return View();
            }

            return RedirectToAction(nameof(Login));
        }
        public ActionResult AddTopic()
        {
            var accessTokenCookie = Request.Cookies.Get("ACCESS_TOKEN");
            var roleCookie = Request.Cookies.Get("ROLE");

            if (accessTokenCookie != null &&
                accessTokenCookie.Value != null &&
                roleCookie != null &&
                roleCookie.Value.Equals(StaticDetails.ROLE_ADMIN))
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

        public async Task<ActionResult> DeleteQuestion()
        {
            var accessTokenCookie = Request.Cookies.Get("ACCESS_TOKEN");
            var roleCookie = Request.Cookies.Get("ROLE");

            if (accessTokenCookie != null &&
                accessTokenCookie.Value != null &&
                roleCookie != null &&
                roleCookie.Value.Equals(StaticDetails.ROLE_ADMIN))
            {
                TestDropDown testDropDown = await RequestService.GetAllTestAndTestIdList(accessTokenCookie.Value);
                if(testDropDown != null)
                    return View(testDropDown);
            }

            return RedirectToAction(nameof(Login));
        }
        [HttpPost]
        public ActionResult DeleteQuestion(string testId)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction(nameof(GetAllQuestions), new { testId });
            }
            return View(testId);
        }
        public async Task<ActionResult> ViewReport()
        {
            var accessTokenCookie = Request.Cookies.Get("ACCESS_TOKEN");
            var roleCookie = Request.Cookies.Get("ROLE");

            if (accessTokenCookie != null &&
                accessTokenCookie.Value != null &&
                roleCookie != null &&
                roleCookie.Value.Equals(StaticDetails.ROLE_ADMIN)
            )
            {
                TestDropDown testDropDown = await RequestService.GetAllTestAndTestIdList(accessTokenCookie.Value);
                if (testDropDown != null)
                    return View(testDropDown);
            }

            return RedirectToAction(nameof(Login));
        }
        [HttpPost]
        public ActionResult ViewReport(string testId)
        {
            if(ModelState.IsValid)
            {
                return RedirectToAction(nameof(StudentReports), new { testId = testId });
            }
            return View();
        }

        //get list of topics
        public async Task<ActionResult> GetAllTopics()
        {
            var accessTokenCookie = Request.Cookies.Get("ACCESS_TOKEN");
            var roleCookie = Request.Cookies.Get("ROLE");

            if (accessTokenCookie != null &&
                accessTokenCookie.Value != null &&
                roleCookie != null &&
                roleCookie.Value.Equals(StaticDetails.ROLE_ADMIN)
            )
            {
                List<Test> testList = await RequestService.GetAllTests(accessTokenCookie.Value);
                if(testList != null)
                    return View(testList);
            }

            return RedirectToAction(nameof(Login));
        }
        public async Task<ActionResult> GetAllQuestions(string testId)
        {
            if(testId != null)
            {
                var accessTokenCookie = Request.Cookies.Get("ACCESS_TOKEN");
                var roleCookie = Request.Cookies.Get("ROLE");

                if (accessTokenCookie != null &&
                    accessTokenCookie.Value != null &&
                    roleCookie != null &&
                    roleCookie.Value.Equals(StaticDetails.ROLE_ADMIN)
                )
                {
                    List<Questions> questionList = await RequestService.GetQuestionsByTestId(testId, accessTokenCookie.Value);
                    ViewBag.TestId = testId;
                    return View(questionList);
                }
            }

            return RedirectToAction(nameof(Login));
        }
        public async Task<ActionResult> DeleteQuestions(string questionId, string testId)
        {
            var accessTokenCookie = Request.Cookies.Get("ACCESS_TOKEN");
            var roleCookie = Request.Cookies.Get("ROLE");

            if (accessTokenCookie != null &&
                accessTokenCookie.Value != null &&
                roleCookie != null &&
                roleCookie.Value.Equals(StaticDetails.ROLE_ADMIN)
            )
            {
                CreateQuestion question = await RequestService.GetQuestionById(questionId, accessTokenCookie.Value);
                if(question != null)
                {
                    ViewBag.QuestionId = questionId;
                    ViewBag.TestId = testId;
                    return View(question);
                }
            }

            return RedirectToAction(nameof(Login));
        }
        [HttpPost]
        public async Task<ActionResult> DeleteQuestionsPost(string questionId, string testId)
        {
            var accessTokenCookie = Request.Cookies.Get("ACCESS_TOKEN");
            var roleCookie = Request.Cookies.Get("ROLE");

            if (accessTokenCookie != null &&
                accessTokenCookie.Value != null &&
                roleCookie != null &&
                roleCookie.Value.Equals(StaticDetails.ROLE_ADMIN)
            )
            {
                if(ModelState.IsValid)
                {
                    bool isDeleteSuccessfull = await RequestService.DeleteQuestion(questionId, testId, accessTokenCookie.Value);
                    if(isDeleteSuccessfull)
                    {
                        return RedirectToAction(nameof(GetAllQuestions), new { testId = testId });
                    }
                }
            }

            return RedirectToAction(nameof(Login));
        }
        //edit question
        public async Task<ActionResult> EditQuestions(string questionId, string testId)
        {
            var accessTokenCookie = Request.Cookies.Get("ACCESS_TOKEN");
            var roleCookie = Request.Cookies.Get("ROLE");

            if (accessTokenCookie != null &&
                accessTokenCookie.Value != null &&
                roleCookie != null &&
                roleCookie.Value.Equals(StaticDetails.ROLE_ADMIN)
            )
            {
                CreateQuestion question = await RequestService.GetQuestionById(questionId, accessTokenCookie.Value);
                if (question != null)
                {
                    ViewBag.QuestionId = questionId;
                    ViewBag.TestId = testId;
                    ViewBag.CurrentDifficultyLevel = question.DifficultyLevel;
                    return View(question);
                }
            }

            return RedirectToAction(nameof(Login));
        }
        [HttpPost]
        public async Task<ActionResult> EditQuestionsPost(string questionId, string testId, CreateQuestion question)
        {
            var accessTokenCookie = Request.Cookies.Get("ACCESS_TOKEN");
            var roleCookie = Request.Cookies.Get("ROLE");

            if (accessTokenCookie != null &&
                accessTokenCookie.Value != null &&
                roleCookie != null &&
                roleCookie.Value.Equals(StaticDetails.ROLE_ADMIN)
            )
            {
                if(ModelState.IsValid)
                {
                    Questions updatedQuestion = await RequestService.UpdateQuestion(questionId, testId, question, accessTokenCookie.Value);
                    if (updatedQuestion != null)
                    {
                        return RedirectToAction(nameof(GetAllQuestions), new { testId = testId });
                    }
                }

                return View(question);
            }

            return RedirectToAction(nameof(Login));
        }

        public ActionResult CreateQuestions(string testId)
        {
            var accessTokenCookie = Request.Cookies.Get("ACCESS_TOKEN");
            var roleCookie = Request.Cookies.Get("ROLE");

            if (accessTokenCookie != null &&
                accessTokenCookie.Value != null &&
                roleCookie != null &&
                roleCookie.Value.Equals(StaticDetails.ROLE_ADMIN)
            )
            {
                ViewBag.TestId = testId;
                return View();
            }

            return RedirectToAction(nameof(Login));
        }
        [HttpPost]
        public async Task<ActionResult> CreateQuestions(string testId, CreateQuestion question)
        {
            if (ModelState.IsValid)
            {
                var accessTokenCookie = Request.Cookies.Get("ACCESS_TOKEN");
                var roleCookie = Request.Cookies.Get("ROLE");

                if (accessTokenCookie != null &&
                    accessTokenCookie.Value != null &&
                    roleCookie != null &&
                    roleCookie.Value.Equals(StaticDetails.ROLE_ADMIN)
                )
                {
                    Questions newCreatedQuestion = await RequestService.CreateQuestion(testId, question, accessTokenCookie.Value);
                    if(newCreatedQuestion != null)
                        return RedirectToAction(nameof(GetAllQuestions), new { testId});
                }
            }
            return View(question);
        }

        public async Task<ActionResult> StudentReports(string testId)
        {
            var accessTokenCookie = Request.Cookies.Get("ACCESS_TOKEN");
            var roleCookie = Request.Cookies.Get("ROLE");

            if (accessTokenCookie != null &&
                accessTokenCookie.Value != null &&
                roleCookie != null &&
                roleCookie.Value.Equals(StaticDetails.ROLE_ADMIN)
            )
            {
                List<ReportsData> reports = await RequestService.GetReportsByTestId(testId, accessTokenCookie.Value);
                if (reports != null)
                    return View(reports);
            }

            return RedirectToAction(nameof(Login));
        }
    }
}


      




 