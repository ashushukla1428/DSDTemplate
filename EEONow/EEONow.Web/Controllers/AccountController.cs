using EEONow.Interfaces;
using EEONow.Models;
using EEONow.Services;
using EEONow.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Newtonsoft.Json;

namespace EEONow.Web.Controllers
{
    [RoutePrefix("Account")]

    public class AccountController : Controller
    {
        IAccountService _accountService;
        ITwoFactorAuthenticationService _TwoFactorAuthenticationService;
        public AccountController()
        {
            _accountService = new AccountService();
            _TwoFactorAuthenticationService = new TwoFactorAuthenticationService();
        }
        // GET: Account
        public ActionResult Login(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                FormsAuthentication.SignOut();
                return Redirect("/");
            }
            LoginModel model = new LoginModel { ReturnUrl = returnUrl };

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("Login");
                }

                var result = await _accountService.Login(model);
                if (result.Succeeded)
                {
                    DeviceInfoRequestModel _deviceInfo = new DeviceInfoRequestModel
                    {
                        UserId = result.UserId,
                        OrganizationId = result.OrgId,
                        UserAgent = Request.ServerVariables["HTTP_USER_AGENT"],
                        RemoteIpAddress = Request.UserHostAddress
                    };
                    if (await _TwoFactorAuthenticationService.CheckTwoFactorAuthentication(_deviceInfo))
                    {
                        string RandomKey = await _TwoFactorAuthenticationService.StoreDeviceInformation(_deviceInfo);
                        if (RandomKey.Length > 0)
                        {
                            var EmailSent = await _TwoFactorAuthenticationService.SendAuthenticationCode(RandomKey);
                            return RedirectToAction("EmailVerification", "Account", new { key = RandomKey, EmailSent = EmailSent });
                        }
                        else
                        {
                            ModelState.AddModelError("", "there was a problem with your Two Factor Authentication (2FA) email verification process, please contact your EEONow systems administrator");
                            return View("Login");
                        }
                    }
                    else
                    {
                        AppUtility.SetCookiesData(result);
                        return RedirectToAction("Index", "Dashboard");
                    }
                }
                else
                {
                    ModelState.AddModelError("", result.Message);
                }

                return View("Login");
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }

        public async Task<ActionResult> ReSendEmailVerification(string key)
        {
            try
            {
                var result = await _TwoFactorAuthenticationService.SendAuthenticationCode(key);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        public ActionResult EmailVerification(string key, bool EmailSent)
        {
            DeviceAuthenticationModel model = new DeviceAuthenticationModel();
            model.RandomKey = key;
            if (EmailSent)
            {
                return View(model);
            }
            else
            {
                return RedirectToAction("Errorwindow", "Home");
            }

        }
        [HttpPost]
        public async Task<ActionResult> EmailVerification(DeviceAuthenticationModel model)
        {
            if (await _TwoFactorAuthenticationService.VerifyTwoFactorAuthenticationCode(model))
            {
                AppUtility.SetCookiesData(await _TwoFactorAuthenticationService.GetUserDetail(model.RandomKey));
                return RedirectToAction("Index", "Dashboard");
            }
            else
            {
                ModelState.AddModelError("", "you have entered the wrong verification code, please contact your EEONow systems administrator");
                return View("EmailVerification", model);
            }


        }
        public ActionResult ForgotPassword()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordModel model)
        {
            try
            {
                var result = await _accountService.ForgotPassword(model);

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Message);
                    return View();
                }
                ViewBag.PasswordSent = "true";
                return View();
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        public ActionResult Logout()
        {
            try
            {
                FormsAuthentication.SignOut();
                AppUtility.ClearDataFromCookie("TempOrgId");
                AppUtility.ClearDataFromCookie("TempRoleId");
                return RedirectToAction("Login");
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        public JsonResult CheckUserAuthenticated()
        {
            return Json(User.Identity.IsAuthenticated, JsonRequestBehavior.AllowGet);
        }
        public ActionResult MenuUi()
        {
            try
            {
                var result = _accountService.BindMenuUi();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        public ActionResult ErrorScreen()
        {
            return View();
        }
        public ActionResult OrganizationUserError()
        {
            return View();
        }
        
        [Authorize()]
        [CustomAuthorizeFilter]
        public async Task<ActionResult> ChangePassword()
        {
            ChangePasswordModel _model = new ChangePasswordModel();
            _model.Email = await _accountService.GetUserEmail();

            return View(_model);
        }
        // [Route("ResetPassword/{Key}")]//HttpUtility.UrlEncode
        [ValidateInput(false)]
        public async Task<ActionResult> ResetPassword(string Key)
        {
            var result = await _accountService.ResetPasswordByUrl(Key);
            result.ResetPasswordKey = Key;
            return View(result);
        }
        [HttpPost]
        //  [Route("ResetPassword/{Key}")]
        public async Task<ActionResult> ResetPassword(ResetPasswordModel model)
        {
            try
            {
                var result = await _accountService.ResetPassword(model);

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Message);
                    return View();
                }
                ViewBag.PasswordSent = "true";
                ViewBag.PasswordSentMessage = result.Message;
                return View();
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        [HttpPost]
        public async Task<ActionResult> ChangePassword(ChangePasswordModel model)
        {
            try
            {
                var result = await _accountService.ChangePassword(model);

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Message);
                    return View();
                }
                ViewBag.PasswordSent = "true";
                return View();
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }


        [ValidateInput(false)]
        public async Task<ActionResult> SetPassword(string Key)
        {
            var result = await _accountService.ResetPasswordByUrl(Key);
            result.ResetPasswordKey = Key;
            return View(result);
        }
        [HttpPost]
        //  [Route("ResetPassword/{Key}")]
        public async Task<ActionResult> SetPassword(ResetPasswordModel model)
        {
            try
            {
                var result = await _accountService.ResetPassword(model);

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Message);
                    return View();
                }
                ViewBag.PasswordSent = "true";
                ViewBag.PasswordSentMessage = result.Message;
                return View();
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
    }
}