
using dsdProjectTemplate.ViewModel.User;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace dsdProjectTemplate.Utility
{
    public class UserSession
    {
        // private constructor
        //int loginId = MySession.Current.LoginId;

        //string property1 = MySession.Current.Property1;
        //MySession.Current.Property1 = newValue;

        //    DateTime myDate = MySession.Current.MyDate;
        //  MySession.Current.MyDate = DateTime.Now;
        private UserSession()
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
                    newUser.FullName = serializeModel.FullName;
                    newUser.FirstName = serializeModel.FirstName;
                    newUser.LastName = serializeModel.LastName;
                    newUser.Roles = serializeModel.Roles;
                    newUser.SelectedOrgId = serializeModel.SelectedOrgId;
                    newUser.Email = serializeModel.Email;
                    newUser.OrgList = serializeModel.OrgList;
                    newUser.CanAddRecords = serializeModel.CanAddRecords;
                    newUser.CanEditRecords = serializeModel.CanEditRecords;
                    HttpContext.Current.User = newUser;

                    // OwnerID = newUser.OwnerID;
                    loggedIn_UserId = serializeModel.UserId;
                    UserRoleId = serializeModel.UserRoleId;
                    Roles = serializeModel.Roles;
                    FullName = serializeModel.FirstName + " " + serializeModel.LastName;
                    UserName = serializeModel.UserName;
                    Email = serializeModel.Email;
                    SelectedOrgId = serializeModel.SelectedOrgId;
                    SelectedOrgName = serializeModel.SelectedOrgName;
                    OrgList = serializeModel.OrgList;
                    CanAddRecords = serializeModel.CanAddRecords;
                    CanEditRecords = serializeModel.CanEditRecords;
                    IsSoftware_User = serializeModel.IsSoftware_User;
                    IsSuperAdmin = serializeModel.IsSuperAdmin;    
                    //IsMainUser = newUser.IsMainUser;
                    //Logo = Logo == "" ? WebConfigurationManager.AppSettings["FilePath"] + "/Content/themes/admin/layout/img/logo.png" : Logo = newUser.Logo;
                }
            }
            else
            {
                Roles = null;
                //loggedIn_UserId = 0;
            }
        }

        // Gets the current session.
        public static UserSession Current
        {
            get
            {
                try
                {
                    UserSession session =
                                     (UserSession)HttpContext.Current.Session["__MySession__"];
                    if (session == null)
                    {
                        session = new UserSession();
                        HttpContext.Current.Session["__MySession__"] = session;
                    }
                    return session;
                }
                catch (Exception)
                {

                    return new UserSession();
                }
            }
        }

        // **** add your session properties here, e.g like this:
        public string Email { get; set; }
        public string UserName { get; set; }
        public long loggedIn_UserId { get; set; }
        public string Roles { get; set; }
        public long UserRoleId { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public long SelectedOrgId { get; set; }
        public string SelectedOrgName { get; set; }
        
        public List<UserOrgList> OrgList { get; set; }
        public bool CanAddRecords { get; set; }
        public bool CanEditRecords { get; set; }
        public bool IsSoftware_User { get; set; }
        public bool IsSuperAdmin { get; set; }

        public static void SetLoginCookiesData(LoginResponse model)
        {   
            FormsAuthentication.SetAuthCookie(model.Email, false);
            var userData = JsonConvert.SerializeObject(model);
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1,
           model.Email, 
           DateTime.Now,
           DateTime.Now.AddMinutes(60),
           true,
           userData,//add only limited data
           FormsAuthentication.FormsCookiePath);

            // Encrypt the ticket.
            string encTicket = FormsAuthentication.Encrypt(ticket);

            // Create the cookie.
            HttpContext.Current.Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));
         
        }
    }
    public class CustomPrincipalSerializeModel
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public long UserId { get; set; }
        public string Roles { get; set; }
        public long UserRoleId { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public long SelectedOrgId { get; set; }
        public string SelectedOrgName { get; set; }
        public List<UserOrgList> OrgList { get; set; }
        public bool CanAddRecords { get; set; }
        public bool CanEditRecords { get; set; }
        public bool IsSoftware_User { get; set; }
        public bool IsSuperAdmin { get; set; }

        public static CustomPrincipalSerializeModel getLoginUserInfo()
        {
            CustomPrincipalSerializeModel _userData = new CustomPrincipalSerializeModel();
            var authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                if (authTicket != null && !authTicket.Expired)
                {
                    _userData = JsonConvert.DeserializeObject<CustomPrincipalSerializeModel>(authTicket.UserData);
                }
            }
            return _userData;
        }
    }
    public class CustomPrincipal : IPrincipal
    {
        public IIdentity Identity { get; private set; }
        public bool IsInRole(string role)
        {
            if (Roles.Any(r => role.Contains(r)))
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

        public string Email { get; set; }
        public string UserName { get; set; }
        public long UserId { get; set; }
        public string Roles { get; set; }
        public long UserRoleId { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public long SelectedOrgId { get; set; }
        public List<UserOrgList> OrgList { get; set; }
        public bool CanAddRecords { get; set; }
        public bool CanEditRecords { get; set; }
    }
}
