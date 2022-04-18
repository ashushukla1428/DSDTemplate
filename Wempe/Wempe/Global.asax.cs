using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using Wempe.Models;
using Wempe.CommonClasses;
using Wempe.Models;


namespace Wempe
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
        }
        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {

                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                        try
                {
                    CustomPrincipalSerializeModel serializeModel = JsonConvert.DeserializeObject<CustomPrincipalSerializeModel>(authTicket.UserData);
                    CustomPrincipal newUser = new CustomPrincipal(authTicket.Name);
                    newUser.UserId = serializeModel.UserId;
                    newUser.FirstName = serializeModel.FirstName;
                    newUser.LastName = serializeModel.LastName;
                    newUser.roles = serializeModel.roles;
                    newUser.OwnerID = serializeModel.OwnerID;
                    HttpContext.Current.User = newUser;
                }
                catch (Exception)
                {
                    
                }
            }
        }



        protected void Application_Error()
        {
            HttpContext httpContext = HttpContext.Current;
            if (httpContext != null)
            {
                RequestContext requestContext = ((MvcHandler)httpContext.CurrentHandler).RequestContext;
                /* when the request is ajax the system can automatically handle a mistake with a JSON response. then overwrites the default response */
                dbWempeEntities _this = new dbWempeEntities();
                
                wmpErrorLog log = new wmpErrorLog { ErrorMessage = requestContext.HttpContext.Error.Message, TimeStamp = DateTime.Now, PageName = System.Web.HttpContext.Current.Request.Url.AbsolutePath };
                _this.wmpErrorLogs.Add(log);
                _this.SaveChanges();

                string page = System.Web.HttpContext.Current.Request.Url.AbsolutePath;
                if (page == "" || page == "/")
                {
                    page = "Login";
                }

                //string html = "<tr><td>Error Message</td><td>Page Name</td></tr>";
                string html = "";


                //html+= "<tr><td>"+ requestContext.HttpContext.Error.Message + "</td><td>" + page + "</td></tr>";

                html += "<p><b> Error Message:</b> " + requestContext.HttpContext.Error.Message + "</p></br></br> <p> <b> Occured at:</b> " + page + "</p>";

                var MailHelper = new MailHelper
                {
                    Sender = ConfigurationManager.AppSettings["EmailFromAddress"], //email.Sender,
                                                                                   // Recipient = "support@watchserve.com"


                     Recipient = ConfigurationManager.AppSettings["ErrorToEmailAddressBcc"],
                    RecipientCC = ConfigurationManager.AppSettings["ErrorToEmailAddressBcc"],

                    Subject = "WEMPE-Error Occured at: " + DateTime.Now,
                    // Body = requestContext.HttpContext.Error.Message + " at page:" + System.Web.HttpContext.Current.Request.Url.AbsolutePath
                    Body = html
                };
                MailHelper.Send();

            }
        }
    }
}