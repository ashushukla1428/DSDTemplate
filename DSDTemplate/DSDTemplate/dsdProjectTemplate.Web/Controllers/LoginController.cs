using dsdProjectTemplate.Web.core;
using dsdProjectTemplate.Services.User.Login;
using dsdProjectTemplate.Services.User.TwoFactorAuthentication;
using dsdProjectTemplate.Utility;
using dsdProjectTemplate.ViewModel.User;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace dsdProjectTemplate.Web.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        private readonly ILoginService _loginService;
        private ManageCookies _manageCookies=new ManageCookies();
        ITwoFactorAuthenticationService _twoFactorAuthenticationService = new TwoFactorAuthenticationService();
        public LoginController()
        {
            _loginService = new LoginService();
        }
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            //TwilioService twilioService = new TwilioService();
            //twilioService.SendSMS();
            var result = await _loginService.LoginAsync(model);

            if (result.Status)
            {
                if(result.SMSTwoFactorAuthentication || result.EmailTwoFactorAuthentication)
                {
                    result.IsTwoFactorAuthenticationRequested = true;
                    var _value = _manageCookies.GetFromCookie("SMSTwoFactorAuthentication", result.UserName.ToString());
                    if (string.IsNullOrEmpty(_value))
                    {
                        result.IsTwoFactorAuthenticationDone = false;

                        await _twoFactorAuthenticationService.SendTwoFactorAuthenticationCodeOnLogin_Async(result.UserId);
                        //_manageCookies.StoreInCookie("SMSTwoFactorAuthentication_Code", "definedFormsWorkFlow", result.UserName.ToString(), "1234",DateTime.Now.AddMinutes(10));

                    }
                    else
                    {
                        result.IsTwoFactorAuthenticationDone = true;
                        UserSession.SetLoginCookiesData(result);
                    }
                }
                else
                {
                    UserSession.SetLoginCookiesData(result);
                }
            }
            return Json(new LoginResponse { 
                Status=result.Status,
                Message=result.Message,
                IsTwoFactorAuthenticationRequested=result.IsTwoFactorAuthenticationRequested,
                IsTwoFactorAuthenticationDone=result.IsTwoFactorAuthenticationDone
            });
        }
        [HttpPost]
        public async Task<ActionResult> CheckTwoFactorAuthentication(LoginViewModel model)
        {
            //TwilioService twilioService = new TwilioService();
            //twilioService.SendSMS();
            var result = await _loginService.LoginAsync(model);

            if (result.Status)
            {
                var _twoFactorAuthenticationCode = Session["TwoFactorAuthenticationCode_OnLogin" + result.UserName.ToString()].ToString();
                    //_manageCookies.GetFromCookie("SMSTwoFactorAuthentication_Code", result.UserName.ToString());
                if (!string.IsNullOrEmpty(_twoFactorAuthenticationCode))
                {
                    if (_twoFactorAuthenticationCode == model.Code)
                    {
                        result.IsTwoFactorAuthenticationRequested = true;
                        result.IsTwoFactorAuthenticationDone = true;
                        _manageCookies.StoreInCookie("SMSTwoFactorAuthentication", "definedFormsWorkFlow", result.UserName.ToString(), "done", DateTime.Now.AddDays(30));
                        UserSession.SetLoginCookiesData(result);
                    }
                    else
                    {
                        result.Status = false;
                        result.Message = "Wrong two factor authentication code!";
                    }
                }
            }
            return Json(result);
        }
        //public void Set(string key, string value, int? expireTime)
        //{
        //    CookieOptions option = new CookieOptions();

        //    if (expireTime.HasValue)
        //        option.Expires = DateTime.Now.AddMinutes(expireTime.Value);
        //    else
        //        option.Expires = DateTime.Now.AddMilliseconds(10);

        //    Response.Cookies.Append(key, value, option);
        //}
        ///// <summary>  
        ///// Delete the key  
        ///// </summary>  
        ///// <param name="key">Key</param>  
        //public void Remove(string key)
        //{
        //    Response.Cookies.Delete(key);
        //}   
    }
}