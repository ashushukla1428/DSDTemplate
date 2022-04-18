using dsdProjectTemplate.Services.SendEmail.User;
using dsdProjectTemplate.Services.User.ForgotPassword;
using dsdProjectTemplate.ViewModel;
using dsdProjectTemplate.ViewModel.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace dsdProjectTemplate.Web.Controllers
{
    public class HomeController : Controller
    {
        
        public ActionResult Index()
        {
            return View();
        }
        public async Task<ActionResult> SetPassword(string key)
        {
            IForgotPasswordService forgotPasswordService = new ForgotPasswordService();
            ViewBag.Key = key;
            return View(await forgotPasswordService.CheckResetPasswordKeyAsync(key));
        }
        [HttpPost]
        //  [Route("ResetPassword/{Key}")]
        [Route("SetPassword")]
        public async Task<ActionResult> SetPassword(ResetPasswordModel model)
        {
            try
            {
                IForgotPasswordService forgotPasswordService = new ForgotPasswordService();
                var result = await forgotPasswordService.ResetPasswordAsync(model);

                if (!result.Status)
                {
                    ModelState.AddModelError("", result.Message);
                    return View();
                }
                ViewBag.PasswordSent = "true";
                ViewBag.PasswordSentMessage = result.Message;
                ViewBag.Key = model.Key;
                model.ConfirmPassword = "";
                model.Password = "";
                return View(model);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        [HttpPost]
        public async Task<ActionResult> SendForgotPasswordLink(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {

                IUserSendEmailService _userSendEmailService = new UserSendEmailService();
                return Json(await _userSendEmailService.SendForgotPasswordMailAsync(model.Email));
            }
            return Json(new ResponseModel { Status = false, Message = ResponseMessages.requiredFields });
        }
        [HttpPost]
        public async Task<ActionResult> SendForgotUserNameLink(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {

                IUserSendEmailService _userSendEmailService = new UserSendEmailService();
                return Json(await _userSendEmailService.SendForgotUserNameMailAsync(model.Email));
            }
            return Json(new ResponseModel { Status = false, Message = ResponseMessages.requiredFields });
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult LogOff()
        {
            Session.Abandon();
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Login");
        }
        public ActionResult NotAuthorized()
        {
            return View();
        }
    }
}