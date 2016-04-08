using EIApp.WebSite.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EIApp.WebSite.Controllers
{
    public class LoginController : Controller
    {
        private EIApp.BLL.UserInfoService bll = new EIApp.BLL.UserInfoService();
        //
        // GET: /Login/

       

        [HttpPost]
        public ActionResult UserLogin(string name)
        {
            if (name != "")
            {
                Session["UserInfo"] = name;
                return Redirect("/Home/Index");
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