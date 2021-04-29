using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace cis237_assignment6.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Application Description";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Kalamazoo Valley Community College";

            return View();
        }
    }
}