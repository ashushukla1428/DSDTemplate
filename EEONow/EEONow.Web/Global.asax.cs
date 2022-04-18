using EEONow.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.UI;

namespace EEONow.Web
{
    //[OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        protected void Application_AuthenticateRequest(Object sender, EventArgs e)
        {
            if (HttpContext.Current.User != null)
            {

                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {

                    if (HttpContext.Current.User.Identity is FormsIdentity)
                    {
                        // Get Forms Identity From Current User
                        FormsIdentity id = (FormsIdentity)HttpContext.Current.User.Identity;
                        // Get Forms Ticket From Identity object
                        FormsAuthenticationTicket ticket = id.Ticket;
                        var data = JsonConvert.DeserializeObject<LoginResponse>(ticket.UserData);
                        string[] roles = data.Roles.Split(',');
                        // Create a new Generic Principal Instance and assign to Current User
                        HttpContext.Current.User = new GenericPrincipal(id, roles);
                    }
                }
            }
        }
        protected void Application_BeginRequest()
        {
            if (!Context.Request.IsSecureConnection && 
                !Context.Request.Url.ToString().Contains("localhost") &&
                !Context.Request.Url.ToString().Contains("dev") &&
                !Context.Request.Url.ToString().Contains("uat"))
            {
                Response.Redirect(System.Configuration.ConfigurationManager.AppSettings["AppURL"]);
            }
        }

    }
}
