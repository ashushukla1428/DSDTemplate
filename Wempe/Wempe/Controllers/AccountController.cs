using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using Wempe.Models;
using Wempe.CommonClasses;
using System.IO;
using System.Web.Configuration;
using System.Data.Entity;

namespace Wempe.Controllers
{
    [Authorize]
    //[InitializeSimpleMembership]
    public class AccountController : Controller
    {
        //
        // GET: /Account/Login
         dbWempeEntities db = new dbWempeEntities();
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }


        [HttpGet]
        public JsonResult getStates(int Id)
        {
            try
            {
                //return Json(db.wmpStates.Where(c => c.IsActive == true && c.OwnerID == SessionMaster.Current.OwnerID && c.CountryId==Id).OrderBy(c => c.stateFullName).Select(c => new { c.Id, c.stateFullName }),JsonRequestBehavior.AllowGet);
                return Json(db.wmpStates.Where(c => c.IsActive == true && c.CountryId == Id).OrderBy(c => c.stateFullName).Select(c => new { c.Id, c.stateFullName }), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(new Result { Status = false, Message = ex.Message });
            }
        }
        //getCities
        [HttpGet]
        public JsonResult getCities(int Id)
        {
            try
            {
                // return Json(db.wmpSampleCities.Where(c => c.IsActive == true && c.OwnerID == SessionMaster.Current.OwnerID && c.StateId == Id).OrderBy(c => c.city).Select(c => new { c.Id, c.city }), JsonRequestBehavior.AllowGet);
                return Json(db.wmpSampleCities.Where(c => c.IsActive == true && c.StateId == Id).OrderBy(c => c.city).Select(c => new { c.Id, c.city }), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(new Result { Status = false, Message = ex.Message });
            }
        }

        [HttpGet]
        public ActionResult MyProfile()
        {
            try
            {
                
                ViewBag.IsEdit = true;
                var _pageData = db.wmpRoleMasters.Where(c => c.IsActive == true && c.OwnerID == SessionMaster.Current.OwnerID).Select(c => new { c.RoleID, c.Role });
                SelectList list = new SelectList(_pageData, "RoleID", "Role");
                ViewBag.Roles = list.OrderBy(s => s.Text);
                //var _pageData = db.wmpRoleMasters.Where(c => c.IsActive == true && c.OwnerID == SessionMaster.Current.OwnerID).Select(c => new { c.RoleID, c.Role });
                //SelectList list = new SelectList(_pageData, "RoleID", "Role");
                //ViewBag.Roles = list;
                // ViewBag["Error"].

                CompanyEmployeeModel rec = (from usr in db.wmpUserMasters
                                            join x in db.wmpEmployees on usr.UserID equals x.UserID
                                            into gj
                                            from com in gj.DefaultIfEmpty()


                                            join role in db.wmpRoleRelationships on usr.UserID equals role.UserID
                                             into gj2
                                            from com2 in gj2.DefaultIfEmpty()


                                            where usr.UserID == SessionMaster.Current.LoginId 
                                            select new CompanyEmployeeModel
                                            {
                                                // CompanyID = com.CompanyID,
                                                employeeFirstName = com.employeeFirstName == null ? usr.FirstName: com.employeeFirstName,
                                                employeeLastName = com.employeeLastName == null ? usr.LastName : com.employeeLastName,
                                                //employee = com.employee,
                                                employeeAddress1 = com.Address1,
                                                employeeAddress2 = com.Address2,
                                                employeePhoneNumber = com.PhoneNumber,
                                                
                                                
                                                  //Phone=com.PhoneNumber,
                                                employeeEmailAddress = com.EmailAddress,
                                                //employeeEmailAddress=usr.e


                                                //Website = com.WebSite,
                                                ImagePath = com.Image != null && com.Image != "" ? com.Image : "/images/nologo.jpg",
                                                employeeRoleID = (int)com2.RoleID,

                                                IsActive = (bool)usr.IsActive,
                                                UserID = usr.UserID,
                                                UserEmail = usr.UserName,
                                                FirstName = usr.FirstName,
                                                LastName = usr.LastName,

                                                CountryId = com.CountryId,
                                                StateId = com.StateId,
                                                CityId = com.CityId,
                                                 ZipCode=com.ZipCode                                                

                                            }).FirstOrDefault();

                ViewBag.Countries = new SelectList(db.wmpCountries.Where(c => c.IsActive == true).OrderBy(c => c.country).Select(c => new { c.Id, c.country }), "Id", "country", 253);

                if (rec.CountryId != null)
                {
                    ViewBag.Countries = new SelectList(db.wmpCountries.Where(c => c.IsActive == true).OrderBy(c => c.country).Select(c => new { c.Id, c.country }), "Id", "country", rec.CountryId);
                    ViewBag.States = new SelectList(db.wmpStates.Where(c => c.IsActive == true && c.CountryId == rec.CountryId).OrderBy(c => c.stateFullName).Select(c => new { c.Id, c.stateFullName }), "Id", "stateFullName", rec.StateId);
                }

                rec.Role = db.wmpRoleMasters.Where(s => s.RoleID == rec.employeeRoleID).FirstOrDefault().Role;
              //  View
                return View(rec);
            }
            catch (Exception ex)
            {
               return  Json(ex.InnerException.Message);
            } 
        }


        [HttpPost]
       // [ValidateAntiForgeryToken]
        public ActionResult UpdateProfile(CompanyEmployeeModel vm)
        {
            ModelState.Remove("Password");
            ModelState.Remove("ConfirmPassword");

            if (db.wmpUserMasters.Any(c => c.UserName == vm.UserEmail && c.UserID != vm.UserID))
            {
                ViewBag.IsEdit = false;
                TempData["Error"] = "username already exists with this email address (" + vm.UserEmail + ") !";
                // return View(vm);

                return RedirectToAction("MyProfile");

                //
            }

            var _pageData = db.wmpRoleMasters.Where(c => c.IsActive == true && c.OwnerID == SessionMaster.Current.OwnerID).Select(c => new { c.RoleID, c.Role });
            SelectList list = new SelectList(_pageData, "RoleID", "Role");
            ViewBag.Roles = list.OrderBy(s => s.Text);
            ViewBag.IsEdit = true;
           // if (ModelState.IsValid)
            {
                try
                {
                    using (TransactionScope scope = new TransactionScope())
                    {
                        // wmpEmployee com = db.wmpEmployees.Where(p => p.Id == SessionMaster.Current.LoginId).FirstOrDefault();
                        wmpEmployee com = db.wmpEmployees.Where(p => p.UserID == SessionMaster.Current.LoginId).FirstOrDefault();
                        if (com != null)
                        {
                            // only admin can change
                            if (vm.employeeFirstName != null)
                            {
                                com.employeeFirstName = vm.employeeFirstName;
                            }
                            else
                            {
                                com.employeeFirstName = vm.FirstName;
                            }
                            if (vm.employeeLastName != null)
                            {
                                com.employeeLastName = vm.employeeLastName;
                            }
                            else
                            {
                                com.employeeLastName = vm.LastName;
                            }

                            if (vm.employeeLastName != null && vm.employeeFirstName != null)
                            {
                                com.employee = vm.employeeFirstName + " " + vm.employeeLastName;
                            }
                            else
                            {
                                com.employee = vm.FirstName + " " + vm.LastName;
                            }
                            //com.IsActive = vm.IsActive;
                            com.EmailAddress = vm.employeeEmailAddress;
                            // only admin can change

                            com.Address1 = vm.employeeAddress1;
                            com.Address2 = vm.employeeAddress2;
                            com.PhoneNumber = vm.employeePhoneNumber;
                            //com.WebSite = vm.Website;

                            com.LastUpdate = DateTime.Now;
                            com.UpdateBy = SessionMaster.Current.LoginId;

                            com.ZipCode = vm.ZipCode;

                            com.CountryId = vm.CountryId;
                            com.StateId = vm.StateId;
                            com.CityId = vm.CityId;

                            //System.Drawing.Image _photo = System.Drawing.Image.FromStream(vm.Image.InputStream);
                            //var _image = ResizeImageClass.ResizeImage(_photo, 200, 200);
                            //Graphics graphics = Graphics.FromImage((Image)_image);
                            //ResizeImageClass.writeWaterMark(graphics, 2, 180, new Font("Arial", 14, FontStyle.Bold), "asdasd");
                            //_image.Save(Server.MapPath("~/Content/CompanyLogo/"+ vm.Image.FileName),System.Drawing.Imaging.ImageFormat.Jpeg);

                            if (vm.Image != null && vm.Image.ContentLength > 0)
                            {
                                //Delete old file
                                if (com.Image != null && com.Image != "")
                                {
                                    if (!Directory.Exists(Server.MapPath(com.Image)))
                                    {
                                        System.IO.File.Delete(Server.MapPath(com.Image));
                                    }
                                }


                                {
                                    string path = Server.MapPath("~/UploadFiles/" + com.OwnerID + "/Employee");

                                    if (!Directory.Exists(path))
                                    {
                                        Directory.CreateDirectory(path);
                                    }

                                    string file = Path.Combine(path, Path.GetFileName(vm.Image.FileName));
                                    vm.Image.SaveAs(file);

                                    com.Image = WebConfigurationManager.AppSettings["FilePath"] + "/UploadFiles/" + com.OwnerID + "/Employee/" + vm.Image.FileName;

                                }
                            }

                            db.Entry(com).State = EntityState.Modified;
                            db.SaveChanges();
                        }

                        wmpUserMaster usr = db.wmpUserMasters.Where(p => p.UserID == vm.UserID).FirstOrDefault();
                        if (usr != null)
                        {
                            // usr.UserName = vm.UserEmail;
                            //  usr.FirstName = vm.FirstName;
                            usr.IsMainUser = SessionMaster.Current.IsMainUser;
                            // usr.LastName = vm.LastName;
                            usr.LastUpdate = DateTime.Now;
                            usr.UpdateBy = SessionMaster.Current.LoginId;
                            // usr.IsActive = vm.IsActive;

                            // sagar 10 dec 2016
                            usr.UserName = vm.UserEmail;
                            usr.FirstName = vm.FirstName;
                            usr.LastName = vm.LastName;
                          
                            // end sagar 10 dec 2016

                            db.Entry(usr).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        scope.Complete();

                        TempData["Error"] = "Profile updated succesfully";
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.IsEdit = true;
                   // ViewBag.Error = "Some error occur";
                    TempData["Error"] = ex.ToString();

                    //return View(vm);

                    return RedirectToAction("MyProfile");
                }
            }
            //else
            //{
            //    ViewBag.IsEdit = true;
            //    return View(vm);
            //}
            ViewBag.Error = "Profile Updated.";
            return RedirectToAction("MyProfile");
            //return Json("Success");
        }


        //
        // POST: /Account/Login

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid && WebSecurity.Login(model.UserName, model.Password, persistCookie: model.RememberMe))
            {
                return RedirectToLocal(returnUrl);
            }

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "The user name or password provided is incorrect.");
            return View(model);
        }

        //
        // POST: /Account/LogOff

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            WebSecurity.Logout();

            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Register

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                try
                {
                    WebSecurity.CreateUserAndAccount(model.UserName, model.Password);
                    WebSecurity.Login(model.UserName, model.Password);
                    return RedirectToAction("Index", "Home");
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/Disassociate

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Disassociate(string provider, string providerUserId)
        {
            string ownerAccount = OAuthWebSecurity.GetUserName(provider, providerUserId);
            ManageMessageId? message = null;

            // Only disassociate the account if the currently logged in user is the owner
            if (ownerAccount == User.Identity.Name)
            {
                // Use a transaction to prevent the user from deleting their last login credential
                using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
                {
                    bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
                    if (hasLocalAccount || OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name).Count > 1)
                    {
                        OAuthWebSecurity.DeleteAccount(provider, providerUserId);
                        scope.Complete();
                        message = ManageMessageId.RemoveLoginSuccess;
                    }
                }
            }

            return RedirectToAction("Manage", new { Message = message });
        }

        //
        // GET: /Account/Manage

        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : "";
            ViewBag.HasLocalPassword = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        //
        // POST: /Account/Manage

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(LocalPasswordModel model)
        {
            bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.HasLocalPassword = hasLocalAccount;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasLocalAccount)
            {
                if (ModelState.IsValid)
                {
                    // ChangePassword will throw an exception rather than return false in certain failure scenarios.
                    bool changePasswordSucceeded;
                    try
                    {
                        changePasswordSucceeded = WebSecurity.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
                    }
                    catch (Exception)
                    {
                        changePasswordSucceeded = false;
                    }

                    if (changePasswordSucceeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                    }
                }
            }
            else
            {
                // User does not have a local password so remove any validation errors caused by a missing
                // OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        WebSecurity.CreateAccount(User.Identity.Name, model.NewPassword);
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    catch (Exception)
                    {
                        ModelState.AddModelError("", String.Format("Unable to create local account. An account with the name \"{0}\" may already exist.", User.Identity.Name));
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/ExternalLogin

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            return new ExternalLoginResult(provider, Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback

        [AllowAnonymous]
        public ActionResult ExternalLoginCallback(string returnUrl)
        {
            AuthenticationResult result = OAuthWebSecurity.VerifyAuthentication(Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
            if (!result.IsSuccessful)
            {
                return RedirectToAction("ExternalLoginFailure");
            }

            if (OAuthWebSecurity.Login(result.Provider, result.ProviderUserId, createPersistentCookie: false))
            {
                return RedirectToLocal(returnUrl);
            }

            if (User.Identity.IsAuthenticated)
            {
                // If the current user is logged in add the new account
                OAuthWebSecurity.CreateOrUpdateAccount(result.Provider, result.ProviderUserId, User.Identity.Name);
                return RedirectToLocal(returnUrl);
            }
            else
            {
                // User is new, ask for their desired membership name
                string loginData = OAuthWebSecurity.SerializeProviderUserId(result.Provider, result.ProviderUserId);
                ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(result.Provider).DisplayName;
                ViewBag.ReturnUrl = returnUrl;
                return View("ExternalLoginConfirmation", new RegisterExternalLoginModel { UserName = result.UserName, ExternalLoginData = loginData });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLoginConfirmation(RegisterExternalLoginModel model, string returnUrl)
        {
            string provider = null;
            string providerUserId = null;

            if (User.Identity.IsAuthenticated || !OAuthWebSecurity.TryDeserializeProviderUserId(model.ExternalLoginData, out provider, out providerUserId))
            {
                return RedirectToAction("Manage");
            }

            if (ModelState.IsValid)
            {
                // Insert a new user into the database
                using (UsersContext db = new UsersContext())
                {
                    UserProfile user = db.UserProfiles.FirstOrDefault(u => u.UserName.ToLower() == model.UserName.ToLower());
                    // Check if user already exists
                    if (user == null)
                    {
                        // Insert name into the profile table
                        db.UserProfiles.Add(new UserProfile { UserName = model.UserName });
                        db.SaveChanges();

                        OAuthWebSecurity.CreateOrUpdateAccount(provider, providerUserId, model.UserName);
                        OAuthWebSecurity.Login(provider, providerUserId, createPersistentCookie: false);

                        return RedirectToLocal(returnUrl);
                    }
                    else
                    {
                        ModelState.AddModelError("UserName", "User name already exists. Please enter a different user name.");
                    }
                }
            }

            ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(provider).DisplayName;
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // GET: /Account/ExternalLoginFailure

        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        [AllowAnonymous]
        [ChildActionOnly]
        public ActionResult ExternalLoginsList(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return PartialView("_ExternalLoginsListPartial", OAuthWebSecurity.RegisteredClientData);
        }

        [ChildActionOnly]
        public ActionResult RemoveExternalLogins()
        {
            ICollection<OAuthAccount> accounts = OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name);
            List<ExternalLogin> externalLogins = new List<ExternalLogin>();
            foreach (OAuthAccount account in accounts)
            {
                AuthenticationClientData clientData = OAuthWebSecurity.GetOAuthClientData(account.Provider);

                externalLogins.Add(new ExternalLogin
                {
                    Provider = account.Provider,
                    ProviderDisplayName = clientData.DisplayName,
                    ProviderUserId = account.ProviderUserId,
                });
            }

            ViewBag.ShowRemoveButton = externalLogins.Count > 1 || OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            return PartialView("_RemoveExternalLoginsPartial", externalLogins);
        }

        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }

        internal class ExternalLoginResult : ActionResult
        {
            public ExternalLoginResult(string provider, string returnUrl)
            {
                Provider = provider;
                ReturnUrl = returnUrl;
            }

            public string Provider { get; private set; }
            public string ReturnUrl { get; private set; }

            public override void ExecuteResult(ControllerContext context)
            {
                OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl);
            }
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}
