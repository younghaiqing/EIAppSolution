using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EIApp.WebSite.Filter
{
    public class MyCustormFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            filterContext.HttpContext.Response.Write("Action执行后");
            base.OnActionExecuted(filterContext);
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //1.获取获取请求的类名和方法名
            string strController = filterContext.RouteData.Values["controller"].ToString();
            string strAction = filterContext.RouteData.Values["action"].ToString();//2.另一种方式 获取请求的类名和方法名
            string strAction2 = filterContext.ActionDescriptor.ActionName;
            string strController2 = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;

            filterContext.HttpContext.Response.Write("Action执行前</br>");
            filterContext.HttpContext.Response.Write("控制器:" + strController + "</br>");
            filterContext.HttpContext.Response.Write("控制器:" + strController2 + "</br>");
            filterContext.HttpContext.Response.Write("Action:" + strAction + "</br>");
            filterContext.HttpContext.Response.Write("Action:" + strAction2 + "</br>");
            base.OnActionExecuting(filterContext);
        }

        /// <summary>
        /// 加载 "视图" 前执行
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnResultExecuting(System.Web.Mvc.ResultExecutingContext filterContext)
        {
            filterContext.HttpContext.Response.Write("加载视图前执行 OnResultExecuting <br/>");
            base.OnResultExecuting(filterContext);
        }

        /// <summary>
        /// 加载"视图" 后执行
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnResultExecuted(System.Web.Mvc.ResultExecutedContext filterContext)
        {
            filterContext.HttpContext.Response.Write("加载视图后执行 OnResultExecuted <br/>");
            base.OnResultExecuted(filterContext);
        }
    }
}