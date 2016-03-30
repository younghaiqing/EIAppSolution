using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EIApp.WebSite.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Login/
        public ActionResult Index()
        {
            if(Session["UserInfo"]==null)
            {
                return Redirect("~/Login/UserLogin");
            }
            else
            { 
            return View();
            }
        }

        [HttpPost]
        public ActionResult UserLogin(string name)
        {
            if (name != "")
            {
                Session["UserInfo"] = name;
                return Redirect("/Login/Index");
            }
            else
            {
                return View();
            }
        }

        public ActionResult UserLogin()
        {
            return View();
        }
    }
}