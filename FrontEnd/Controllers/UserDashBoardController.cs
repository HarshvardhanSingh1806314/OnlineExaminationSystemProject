using FrontEnd.AsyncServices;
using FrontEnd.Models;
using FrontEnd.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;


namespace FrontEnd.Controllers
{
    public class UserDashBoardController : Controller
    {
        // GET: UserDashBoard
        public async Task<ActionResult> Index()
        {
            var accessTokenCookie = Request.Cookies.Get("ACCESS_TOKEN");
            var roleCookie = Request.Cookies.Get("ROLE");

            if(accessTokenCookie != null &&
               accessTokenCookie.Value != null &&
               roleCookie != null &&
               roleCookie.Value.Equals(StaticDetails.ROLE_STUDENT)
            )
            {
                List<UserTest> testList = await RequestService.GetTests(accessTokenCookie.Value);
                if (testList != null)
                {
                    ViewBag.Username = StaticDetails.USERNAME;
                    return View(testList);
                }
            }
            return RedirectToActionPermanent("Login", "User");
        }
        public async Task<ActionResult> StudentReportsuser()
        {
            var accessTokenCookie = Request.Cookies.Get("ACCESS_TOKEN");
            var roleCookie = Request.Cookies.Get("ROLE");

            if (accessTokenCookie != null &&
               accessTokenCookie.Value != null &&
               roleCookie != null &&
               roleCookie.Value.Equals(StaticDetails.ROLE_STUDENT)
            )
            {
                List<StudentReport> studentReports = await RequestService.GetAllStudentReports(accessTokenCookie.Value);
                if(studentReports != null)
                {
                    return View(studentReports);
                }
            }

            return RedirectToActionPermanent("Login", "User");
        }

        public async Task<ActionResult> ExamDisplay(string testId, string difficultyLevel)
        {
            if(testId == null && difficultyLevel == null)
            {
                return RedirectToActionPermanent(nameof(Index));
            }
            var accessTokenCookie = Request.Cookies.Get("ACCESS_TOKEN");
            var roleCookie = Request.Cookies.Get("ROLE");

            if (accessTokenCookie != null &&
               accessTokenCookie.Value != null &&
               roleCookie != null &&
               roleCookie.Value.Equals(StaticDetails.ROLE_STUDENT)
            )
            {
                List<Questions> questionList = await RequestService.GetQuestionsByTestIdAndDifficultyLevel(testId, difficultyLevel, accessTokenCookie.Value);
                if(questionList != null)
                {
                    //ViewBag.Duration = duration;
                    
                    return View(questionList);
                }
            }

            return RedirectToActionPermanent("Login", "User");
        }

        [HttpPost]
        public async Task<JsonResult> Submit(string testId, string difficultyLevel, List<QuestionResponse> questionResponses)
        {
            var accessTokenCookie = Request.Cookies.Get("ACCESS_TOKEN");
            var roleCookie = Request.Cookies.Get("ROLE");

            if (accessTokenCookie != null &&
               accessTokenCookie.Value != null &&
               roleCookie != null &&
               roleCookie.Value.Equals(StaticDetails.ROLE_STUDENT)
            )
            {
                SubmitQuestionResponse submitQuestionResponse = new SubmitQuestionResponse
                {
                    QuestionResponses = questionResponses
                };
                Result result = await RequestService.SubmitTest(testId, difficultyLevel, submitQuestionResponse, accessTokenCookie.Value);
                if (result != null)
                {
                    TempData["result"] = result;
                    if(result.TestResult == "PASSED")
                    {
                        if (difficultyLevel.ToLower() == "easy")
                            difficultyLevel = "medium";
                        else if (difficultyLevel.ToLower() == "medium")
                            difficultyLevel = "hard";
                        else if(difficultyLevel.ToLower() == "hard")
                        {
                            testId = null;
                            difficultyLevel = null;
                        }
                        return Json(new { redirectUrl = Url.Action("ShowResult", "UserDashBoard", new { testId, difficultyLevel})}, "application/json");
                    }
                    else
                    {
                        return Json(new { redirectUrl = Url.Action("ShowResult", "UserDashBoard") });
                    }
                }
            }
            return Json(new { redirectUrl = Url.Action("Login", "User") });
        }
        public ActionResult ShowResult(string testId = null, string difficultyLevel = null)
        {
            Result result = (Result)TempData["result"];
            ViewBag.TestId = testId;
            ViewBag.DifficultyLevel = difficultyLevel;
            return View(result);
        }
        public ActionResult Levels()
        {
            return View();
        }
    }
}