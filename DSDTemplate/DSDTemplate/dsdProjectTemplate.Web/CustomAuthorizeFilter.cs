using dsdProjectTemplate.Services.User;
using dsdProjectTemplate.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace dsdProjectTemplate.Web
{
    public class CustomAuthorizeFilter : AuthorizeAttribute
    {
         
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var isAuthorized = base.AuthorizeCore(filterContext.HttpContext);

            if (!isAuthorized)
            {
                filterContext.Result = new RedirectResult("~/Home/NotAuthorized"); //User not Authorized
            }

            else
            {

                HttpContextBase context = new HttpContextWrapper(HttpContext.Current);
                RouteData rd = RouteTable.Routes.GetRouteData(context);

                // if use is IsSuperAdmin then no need to check any rights
                if (!UserSession.Current.IsSuperAdmin)
                {
                    if (rd != null)
                    {
                        string controllerName = rd.GetRequiredString("controller");
                        string actionName = rd.GetRequiredString("action");
                        if(actionName== "Index") 
                        {
                        if (!UserService.IsValidateUserRole(controllerName, actionName))
                        {
                            filterContext.Result = new RedirectResult("~/Home/NotAuthorized");

                        }
                        }
                    }
                }


            }


        }
    }
}