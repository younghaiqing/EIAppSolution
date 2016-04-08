using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EIApp.WebSite.Filter
{
    public class LoginFilter : ActionFilterAttribute
    {
        /// <summary>
        /// Action执行前
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            if (filterContext.HttpContext.Session["UserInfo"] == null)
            {
                filterContext.HttpContext.Response.Redirect("~/Login/UserLogin");
            }
           
        }
    }
}