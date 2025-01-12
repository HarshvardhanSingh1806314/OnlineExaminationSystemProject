using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC.Controllers
{
    public class AdminPageController : Controller
    {
        // GET: AdminPage
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AddTopic()
        {
            ViewBag.Message = "Your User page.";
            return View();
        }
        public ActionResult AddQuestion()
        {
            ViewBag.Message = "Your User page.";
            return View();
        }
        public ActionResult DeleteQuestion()
        {
            ViewBag.Message = "Your User page.";
            return View();
        }
        public ActionResult ViewReports()
        {
            ViewBag.Message = "Your User page.";
            return View();
        }
    }
}