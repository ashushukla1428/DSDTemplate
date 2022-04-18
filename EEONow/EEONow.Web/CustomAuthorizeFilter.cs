using EEONow.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace EEONow.Web
{
    public class CustomAuthorizeFilter : AuthorizeAttribute
    {
        //protected override bool AuthorizeCore(HttpContextBase httpContext)
        //{

        //    var isAuthorized = base.AuthorizeCore(httpContext);

        //    if (!isAuthorized)
        //    {
        //        return false; //User not Authorized
        //    }

        //    else
        //    {
        //        string url = HttpContext.Current.Request.Url.ToString();
        //        HttpContextBase context = new HttpContextWrapper(HttpContext.Current);
        //        RouteData rd = RouteTable.Routes.GetRouteData(context);

        //        if (rd != null)
        //        {
        //            string controllerName = rd.GetRequiredString("controller");
        //            string actionName = rd.GetRequiredString("action");
        //            return AccountService.ValidateUserRole(controllerName, actionName);
        //        }
        //        return false;
        //        //Check your conditions here
        //    }
        //}
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var isAuthorized = base.AuthorizeCore(filterContext.HttpContext);

            if (!isAuthorized)
            {
                filterContext.Result = new RedirectResult("~/Home/NotAuthorized"); //User not Authorized
            }

            else
            {
                string url = HttpContext.Current.Request.Url.ToString();
                HttpContextBase context = new HttpContextWrapper(HttpContext.Current);
                RouteData rd = RouteTable.Routes.GetRouteData(context);

                if (rd != null)
                {
                    string controllerName = rd.GetRequiredString("controller");
                    string actionName = rd.GetRequiredString("action");
                    if(!AccountService.ValidateUserRole(controllerName, actionName))
                    {
                        filterContext.Result = new RedirectResult("~/Home/NotAuthorized");

                    }
                }
               // filterContext.Result = new RedirectResult("~/Home/Index");
                //Check your conditions here
            }
             

        }
    }
}