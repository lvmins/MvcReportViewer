using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GPCT_Coin.Models
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class FilterHelper : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var user = filterContext.HttpContext.User.Identity.Name;
            //string webFoot = HttpHelper.GetApplicationCurrentFootVirtualPath();
            if (string.IsNullOrEmpty(user))
            {
                string returnUrl = HttpContext.Current.Request.Url.ToString();
                filterContext.Result = new RedirectResult("~/Login/Index?returnUrl=" + returnUrl);//Session为空时候，跳到登录页面
                //filterContext.HttpContext.Response.Redirect("/Login/Index");
            }
        }
    }
}