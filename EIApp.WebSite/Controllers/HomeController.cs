using EIApp.WebSite.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EIApp.WebSite.Controllers
{
    public class HomeController : Controller
    {
        [LoginFilter]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Main()
        {
            if (Session["UserInfo"] == null)
            {
                return Redirect("~/Login/UserLogin");
            }
            else
            {
                return View();
            }
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}