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
        public JsonResult UserLogin(EIApp.Models.UserInfo model)
        {
            int intret = 0;
            if (model != null && !string.IsNullOrEmpty(model.UName) && !string.IsNullOrEmpty(model.UName))
            {
                var LoginUser = bll.LoadEntities(m => m.UName.Equals(model.UName)).FirstOrDefault();
                if (LoginUser != null && LoginUser.Pwd.Equals(model.Pwd))
                {
                    Session["UserInfo"] = model;
                    intret = 1;
                }
            }
            return Json(intret, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UserLogin()
        {
            return View();
        }
    }
}