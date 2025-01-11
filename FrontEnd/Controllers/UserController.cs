using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FrontEnd.Models;

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
            ViewBag.Message = "Your User page.";
            return View();
        }
        [HttpPost]
        public ActionResult Login(Student s)
        {
            return View();
        }
        public ActionResult Register()
        {
            ViewBag.Message = "Your User Register page.";

            return View(new Student());
        }

        [HttpPost]
        public ActionResult Register(Student student)
        {
            return View(student);
        }
        public ActionResult Reset()
        {
            ViewBag.Message = "Your User Register page.";

            return View(new Resetpassword());
        }

        [HttpPost]
        public ActionResult Reset(Resetpassword resetpassword)
        {
            return View(resetpassword);
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