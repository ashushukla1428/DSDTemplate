using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;
using Wempe.Models;

namespace Wempe.CommonClasses
{
    public class SessionMaster
    { // private constructor
        private SessionMaster()
        {
            HttpCookie authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {

                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);

                CustomPrincipalSerializeModel serializeModel = JsonConvert.DeserializeObject<CustomPrincipalSerializeModel>(authTicket.UserData);
                CustomPrincipal newUser = new CustomPrincipal(authTicket.Name);
                if (serializeModel != null)
                {
                    newUser.UserId = serializeModel.UserId;
                    newUser.FirstName = serializeModel.FirstName;
                    newUser.LastName = serializeModel.LastName;
                    newUser.roles = serializeModel.roles;
                    newUser.OwnerID = serializeModel.OwnerID;
                    HttpContext.Current.User = newUser;
                    OwnerID = newUser.UserId;
                    LoginId = newUser.OwnerID;
                    IsMainUser = newUser.IsMainUser;
                    Logo = Logo == "" ? WebConfigurationManager.AppSettings["FilePath"] + "/Content/themes/admin/layout/img/logo.png" : Logo = newUser.Logo;
                }
            }
            else
            {
                OwnerID = 0;
                LoginId = 0;
                Logo = WebConfigurationManager.AppSettings["FilePath"] + "/Content/themes/admin/layout/img/logo.png";

            }
        }

        // Gets the current session.
        public static SessionMaster Current
        {
            get
            {
                SessionMaster session = (SessionMaster)HttpContext.Current.Session["__SessionMaster__"];
                if (session == null)
                {
                    //if (session.themeName == null)
                    //{
                    //    session.themeName = "darkblue.css";
                    //}
                    session = new SessionMaster();
                    HttpContext.Current.Session["__SessionMaster__"] = session;
                }

                return session;
            }
        }

        // **** add your session properties here, e.g like this:

        public Int64 LoginId { get; set; }
        public Int64 OwnerID { get; set; }
        public bool IsMainUser { get; set; }
        public string Logo { get; set; }


        public string StripLogo { get; set; }
        public string themeName { get; set; }

        public string Name { get; set; }

        public string Role { get; set; }


    }
}