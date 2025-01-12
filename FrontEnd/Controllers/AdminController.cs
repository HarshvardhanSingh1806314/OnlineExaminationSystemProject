﻿using FrontEnd.AsyncServices;
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

            return View(new Tests());
        }
        [HttpPost]
        public ActionResult AddTopic(Tests test)
        {


            return View(test);
        }
        public ActionResult AddQuestion()
        {
            ViewBag.Message = "Your Add Question page.";

            return View(new Questions());
        }
        [HttpPost]
        public ActionResult AddQuestion(Questions question)
        {


            return View(question);
        }
        public ActionResult DeleteQuestion()
        {
            return View();
        }
        public ActionResult ViewReport()
        {
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
            //List<Questions>  QuestionsList = db.Questions.ToList();
            //return View(QuestionList);

            return View();
        }
        public ActionResult StudentReports()
        {
            return View();
        }
    }
}


      




 