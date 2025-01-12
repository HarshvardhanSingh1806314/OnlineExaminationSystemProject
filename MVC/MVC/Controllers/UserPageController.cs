using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC.Controllers
{
    public class UserPageController : Controller
    {
        // GET: UserPage
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Report()
        {
            ViewBag.Message = "Your User page.";
            return View();
        }
        public ActionResult Exam()
        {
            ViewBag.Message = "Your User page.";
            return View();
        }
    }
}