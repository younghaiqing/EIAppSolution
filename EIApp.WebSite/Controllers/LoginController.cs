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

        public ActionResult Index()
        {
            if (Session["UserInfo"] == null)
            {
                return Redirect("~/Login/UserLogin");
            }
            else
            {
                //var bll=new EIApp.BLL.UserInfoService();
                //var modelList = bll.LoadEntities(m=>1==1);
                return View();
            }
        }

        public JsonResult GetUserList(int page, int rows, string sort, string order)
        {
            int total = 0;
            bool isAsc = true;
            if (order != null && order.Equals("asc"))
            {
                isAsc = true;
            }
            else
            {
                isAsc = false;
            }
            //var modelList = bll.LoadEntities(m => 1 == 1);
            var modelList = bll.LoadPageEntities(page, rows, out total, m => 1 == 1
                , isAsc, m => new { m.ID });
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["total"] = total;
            dic["rows"] = modelList;
            return Json(dic);
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
            ViewBag.Message = "this is Example!";
            ViewData["Message"] = "test";
            return View();
        }

        [HttpPost]
        public string Say()
        {
            return "Hello,World!";
        }

        #region 创建

        public ActionResult Create()
        {
            return PartialView("CreateUserInfo");
            //return View();
        }

        [HttpPost]
        public JsonResult Create(EIApp.Models.UserInfo model)
        {
            if (bll.AddEntity(model).ID > 0)
            {
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion 创建

        [HttpPost]
        public JsonResult Delete(int ID)
        {
            if (bll.DeleteEntity(new EIApp.Models.UserInfo() { ID = ID }) == true)
            {
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }
    }
}