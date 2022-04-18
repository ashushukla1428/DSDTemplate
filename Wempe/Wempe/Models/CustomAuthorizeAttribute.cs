using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.UI;
using Wempe.CommonClasses;
// [CustomAuthorize()]
namespace Wempe.Models
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        public string UsersConfigKey { get; set; }
        public string RolesConfigKey { get; set; }
    //    private bool _requestType = true;
        protected virtual CustomPrincipal CurrentUser
        {
            get { return HttpContext.Current.User as CustomPrincipal; }
        }

        protected virtual void OnActionExecuting(
    ActionExecutingContext filterContext
)
        {

        }

        //public  void OnActionExecuting(ActionExecutingContext filterContext)
        //{
        //    // Ensure ASP.NET Simple Membership is initialized only once per app start
        //    //LazyInitializer.EnsureInitialized(ref _initializer, ref _isInitialized, ref _initializerLock);
        //}


        public override void OnAuthorization(AuthorizationContext filterContext)
        {
           if(filterContext.HttpContext.Application.Count==0)
            {
                

                //ScriptManager.RegisterStartupScript(this, Page.GetType(), "key", "MyFunc()", true);
            }


            if (SessionMaster.Current.LoginId == 0)
             {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.HttpContext.Response.StatusCode = 403;
                    filterContext.Result = new JsonResult
                    {
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                        Data = new Result { Status = false, Message = "Session TimeOut. Please login again." }
                    };
                }
                else
                {

                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "Login" }));
                }
                return;

                // filterContext.HttpContext.Response.BufferOutput = true;
                //filterContext.RequestContext.HttpContext.Response.Redirect("/");
                //Environment.Exit(1);
                //filterContext.Result = new JsonResult()
                //{
                //    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                //    Data = new Result { Status = false, Message = "Session TimeOut. Please login again." }
                //};


                //filterContext.RequestContext.HttpContext.Response.Redirect("/");
                //filterContext.RequestContext.HttpContext.Response.StatusCode
                //TempData["SessionTimeout"] = "Your session has been timeout. Please login again.";
                // HttpContext.Current.Response.Redirect("/");

                // if(filterContext.)




                //filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "Login" }));



                //return;
                // return view
            }

            if (filterContext.HttpContext.Request.IsAuthenticated)
            {
                var authorizedUsers = ConfigurationManager.AppSettings[UsersConfigKey];
                var authorizedRoles = ConfigurationManager.AppSettings[RolesConfigKey];
                string requestType = filterContext.HttpContext.Request.Headers["requestType"];
                Users = String.IsNullOrEmpty(Users) ? authorizedUsers : Users;
                Roles = String.IsNullOrEmpty(Roles) ? authorizedRoles : Roles;
                dbWempeEntities _this = new dbWempeEntities();
                var _data = _this.Database.SqlQuery<Int32>("USP_CheckAuthorization @p0, @p1, @p2", filterContext.ActionDescriptor.ControllerDescriptor.ControllerName, filterContext.ActionDescriptor.ActionName, CurrentUser.UserId).FirstOrDefault();


                if (_data == 0)
                {
                    if (requestType == "client")
                    {
                        filterContext.Result = new JsonResult()
                        {
                            JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                            Data = new Result { Status = false, Message = Messages.accessDenied }
                        };
                    }
                    else
                    {
                        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Error", action = "AccessDenied" }));
                    }
                }



                /*
                if (!String.IsNullOrEmpty(Users))
                {
                    if (!Users.Contains(CurrentUser.UserId.ToString()))
                    {
                        filterContext.Result = new RedirectToRouteResult(new
                        RouteValueDictionary(new { controller = "Error", action = "AccessDenied" }));

                        // base.OnAuthorization(filterContext); //returns to login url
                    }
                    
                }*/
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "Login" }));
            }

        }
    }
    public class CustomPrincipal : IPrincipal
    {
        public IIdentity Identity { get; private set; }
        public bool IsInRole(string role)
        {
            if (roles.Any(r => role.Contains(r)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public CustomPrincipal(string Username)
        {
            this.Identity = new GenericIdentity(Username);
        }

        public Int64 UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string[] roles { get; set; }
        public Int64 OwnerID { get; set; }
        public bool IsMainUser { get; set; }
        public string Logo { get; set; }
    } 
}