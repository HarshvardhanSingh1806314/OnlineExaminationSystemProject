using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using FrontEnd.AsyncServices;
using FrontEnd.Models;
using FrontEnd.Utility;
using Newtonsoft.Json;
using static FrontEnd.Models.ResponseModels;

namespace FrontEnd.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            ViewBag.CurrentQuestion = 1;
            ViewBag.TotalQuestions = 10;
            return View();
        }
        public ActionResult Login()
        {
            if(Request.Cookies.Get("ACCESS_TOKEN") != null)
            {
                Response.Cookies.Add(new HttpCookie("ACCESS_TOKEN")
                {
                    Expires = DateTime.Now.AddDays(-1)
                });
            }

            if(Request.Cookies.Get("ROLE") != null)
            {
                Response.Cookies.Add(new HttpCookie("ROLE") { 
                    Expires = DateTime.Now.AddDays(-1)
                });
            }
            ViewBag.Message = "Your User page.";
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Login(StudentLogin  studentlogin)
        {
            if(ModelState.IsValid)
            {
                LoginModel loggedInUser = await RequestService.StudentLoginServive(studentlogin);
                string roleHash = IdGenerator.GenerateRoleId("STUDENT");
                StaticDetails.ROLE_STUDENT = roleHash;
                HttpCookie accessTokenCookie = new HttpCookie("ACCESS_TOKEN", loggedInUser.AccessToken)
                {
                    HttpOnly = true,
                    Secure = true,
                    Expires = DateTime.Now.AddDays(1)
                };
                Response.Cookies.Add(accessTokenCookie);
                HttpCookie roleCookie = new HttpCookie("ROLE", roleHash)
                {
                    HttpOnly = true,
                    Expires = DateTime.Now.AddDays(1)
                };
                Response.Cookies.Add(roleCookie);
                StaticDetails.USERNAME = loggedInUser.Username;
                return RedirectToAction(nameof(Index), "UserDashBoard");
            }
            return View(studentlogin);
        }
        public ActionResult Register()
        {
            ViewBag.Message = "Your User Register page.";

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Register(Student student)
        {
            if(ModelState.IsValid)
            {
                if (await RequestService.StudentRegisterService(student))
                    return RedirectToAction(nameof(Login));
            }

            return View(student);
        }
        public ActionResult Reset()
        {
            ViewBag.Message = "Your User Register page.";

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Reset(Resetpassword resetpassword)
        {
            bool resetPasswordSuccess = await RequestService.ResetStudentPassword(resetpassword);
            if (resetPasswordSuccess)
                return RedirectToActionPermanent(nameof(Login));

            return View();
        }

        public ActionResult Logout()
        {
            if(Request.Cookies.Get("ACCESS_TOKEN") != null)
            {
                Response.Cookies.Add(new HttpCookie("ACCESS_TOKEN")
                {
                    Expires = DateTime.Now.AddDays(-1)
                });
            }

            if(Request.Cookies.Get("ROLE") != null)
            {
                Response.Cookies.Add(new HttpCookie("ROLE")
                {
                    Expires = DateTime.Now.AddDays(-1)
                });
            }

            return RedirectToActionPermanent(nameof(Login));
        }
        public ActionResult Question(string selectedOption, string action)
        {
            // Handle selected answer (e.g., save to database, validate answer)
            if (action == "Next")
            {
                // Logic for moving to the next question
            }
            else if (action == "Finish")
            {
                // Logic for finishing the quiz
            }
            return RedirectToAction("Index");
        }
    }
}