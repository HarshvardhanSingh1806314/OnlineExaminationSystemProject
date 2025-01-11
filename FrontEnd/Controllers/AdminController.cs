using FrontEnd.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
            ViewBag.Message = "Your Admin page.";

            return View(new Admin());
        }

        [HttpPost]
        public ActionResult Login(Admin admin)
        {
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


      




 