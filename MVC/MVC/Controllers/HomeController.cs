using MVC.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        //[HttpPost]
        //public ActionResult AdminPage(Admin admin)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        adminRepository.Create(admin);
        //        return RedirectToAction("Admin");
        //    }
        //    return View(admin);
        //}
        public ActionResult AdminPage()
        {
            ViewBag.Message = "Your Admin page.";

            return View(new Admin ());
        }
        
       
       

        // POST: Handle Login Form Submission
        //[HttpPost]
//        public ActionResult AdminPage(string Email, string Password)
//        {
//            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
//            {
//                ViewBag.Message = "Email and Password are required.";
//                return View("AdminPage");
//            }

//            try
//            {
                
//                using (var db = new AdminDbContext()) // Replace with your DB context class
//                {
//                    Admin user = new Admin
//                    {
//                        Email = Email,
//                        Password = Password
//                    };

//                    // Save to the database
//                    db.AdminUsers.Add(user);
//                    db.SaveChanges();

//                    ViewBag.Message = "Login successful and data stored!";
//                    return RedirectToAction("Dashboard", "Admin"); // Redirect to the admin dashboard
//                }
//            }
//            catch
//            {
//                ViewBag.Message = "An error occurred while storing the data.";
//                return View("AdminPage");
//            }
//        }

        
//    }
//}

//public ActionResult Admin()
//        {
//            ViewBag.Message = "Your Admin page.";
//            return View();
//        }

//        public ActionResult UserPage()
//        {
//            ViewBag.Message = "Your User page.";
//            return View();
//        }
       
    }
}
