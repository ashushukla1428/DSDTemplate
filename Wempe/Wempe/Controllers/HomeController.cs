using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Transactions;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI;
using Wempe.CommonClasses;
using Wempe.Models;


namespace Wempe.Controllers
{
    public class HomeController : Controller
    {
        dbWempeEntities _this = new dbWempeEntities();
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        public ActionResult Login(string ReturnUrl)
        {

           //model.EmailAddress = "abc@gmail.com";
           //string html = RazorViewToString.RenderRazorViewToString(this, "~/Views/emailTemplate/subscriptionsEmail.cshtml", model);

            return View();
        }

        private void writePDF()
        {
            string HTMLContent = "Hello <b>World</b>";

            //Response.Clear();
            //Response.ContentType = "application/pdf";
            //Response.AddHeader("content-disposition", "attachment;filename=" + "PDFfile.pdf");
            //Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //Response.BinaryWrite(GetPDF(HTMLContent));
            //Response.End();
            var _fileData = GetPDF(new WebClient().DownloadString("http://localhost:52871/Temp.html"));
            FileStream fs = new FileStream(Server.MapPath("/Upload/eMailArchive/LetterToCustomer.pdf"), FileMode.OpenOrCreate);
            fs.Write(_fileData, 0, _fileData.Length);
            fs.Close();

        }
        public byte[] GetPDF(string pHTML)
        {
            byte[] bPDF = null;

            MemoryStream ms = new MemoryStream();

            string TempHTML = pHTML;
           // TempHTML=TempHTML.Replace()
            TextReader txtReader = new StringReader(pHTML);
            
            // 1: create object of a itextsharp document class
            Document doc = new Document(PageSize.A4, 25, 25, 25, 25);
            
            // 2: we create a itextsharp pdfwriter that listens to the document and directs a XML-stream to a file
            PdfWriter oPdfWriter = PdfWriter.GetInstance(doc, ms);
            var userpass = Encoding.ASCII.GetBytes("userpass");
            var ownerpass = Encoding.ASCII.GetBytes("ownerpass");
            oPdfWriter.SetEncryption(userpass, ownerpass, 255, true);

           // XMLWorkerHelper.GetInstance().ParseXHtml(oPdfWriter, doc, txtReader);
            // 3: we create a worker parse the document
            HTMLWorker htmlWorker = new HTMLWorker(doc);

            // 4: we open document and start the worker on the document
            doc.Open();
            htmlWorker.StartDocument();

            // 5: parse the html into the document
            htmlWorker.Parse(txtReader);

            // 6: close the document and the worker
            htmlWorker.EndDocument();
            htmlWorker.Close();
            doc.Close();

            bPDF = ms.ToArray();

            return bPDF;
        }


        [HttpPost]
        public ActionResult Login(LoginUserModel model)
        {

            // int i = Convert.ToInt32("saaassd");
            //var tt = Helper.ComputeHash("test", "SHA512", null);
            // SessionMaster.Current.themeName = "blue.css";
            if (ModelState.IsValid)
            {
                var _data = _this.wmpUserMasters.FirstOrDefault(c => c.UserName == model.EmailAddress );
                
                // return RedirectToAction("NewRepair", "Repair");




                if (_data != null)
                {//model.Password == _data.Password



                    if (_data.Role == "Company")
                    {
                        if (_data.wmpCompanyMasters.FirstOrDefault().IsActive == false)
                        {
                            ModelState.AddModelError("LogOnError", "Your company account is deactivate by super admin");
                            return View(model);
                        }
                    }

                    if (_data.IsActive==false)
                    {

                        ModelState.AddModelError("LogOnError", "Your account is deactivate by super admin");
                        return View(model);
                    }

                   

                    if (Helper.VerifyHash(model.Password,"SHA512",_data.Password))
                    {
                        if (_data.IsMainUser == false)
                        {
                            var companyInfo = _this.wmpCompanyPackages.Where(s => s.OwnerId == _data.OwnerID).OrderByDescending(c => c.Id).FirstOrDefault();

                            if (companyInfo != null)
                            {
                                if (companyInfo.PackageRenewalDate != "" || companyInfo.PackageRenewalDate != null)
                                {
                                    TimeSpan difference = Convert.ToDateTime(companyInfo.PackageRenewalDate) - DateTime.Now;
                                    if (difference.Days < 0)
                                    //if (Convert.ToDateTime(companyInfo.PackageRenewalDate).t <= DateTime.Now.)
                                    {

                                        ModelState.AddModelError("LogOnError", "Your Company Account Has Expired");
                                        return View(model);
                                    }
                                }
                            }
                        }
                        //FormsAuthentication.SetAuthCookie(model.EmailAddress, true);
                        CustomPrincipalSerializeModel serializeModel = new CustomPrincipalSerializeModel();
                        serializeModel.UserId = _data.UserID;
                        serializeModel.FirstName = _data.FirstName;
                        serializeModel.LastName = _data.LastName;
                        serializeModel.IsMainUser = (bool)_data.IsMainUser;
                   //  serializeModel.roles = _data.Role.Split(',');

                        serializeModel.OwnerID = Convert.ToInt64(_data.OwnerID);

                        // wmpUserTrack trck = new wmpUserTrack { Type = "Login", TimeStamp=DateTime.Now, Uid = Convert.ToInt32(serializeModel.UserId) };

                        wmpUserTrack trck = new wmpUserTrack {  LoginTime = DateTime.UtcNow, Uid = Convert.ToInt32(serializeModel.UserId) };
                        _this.wmpUserTracks.Add(trck);
                        _this.SaveChanges();



                        SessionMaster.Current.LoginId = serializeModel.UserId;
                        SessionMaster.Current.OwnerID = serializeModel.OwnerID;
                        SessionMaster.Current.IsMainUser =(bool)_data.IsMainUser;
                        SessionMaster.Current.Logo = null;
                        SessionMaster.Current.StripLogo = null;

                        TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;

                        string TempName = textInfo.ToTitleCase(serializeModel.FirstName + " " + serializeModel.LastName); //War And Peace


                        wmpRoleRelationship a = _this.wmpRoleRelationships.Where(s => s.UserID == serializeModel.UserId).FirstOrDefault();
                        wmpRoleMaster obj = _this.wmpRoleMasters.Where(s => s.RoleID == a.RoleID).FirstOrDefault();

                        SessionMaster.Current.Name = TempName;

                        SessionMaster.Current.Role = obj.Role;



                        string userData = JsonConvert.SerializeObject(serializeModel);
                        if (_data.IsMainUser == false)
                        {
                            var _companyData = _this.wmpCompanyMasters.Where(c => c.OwnerId == _data.OwnerID).FirstOrDefault();
                            if (_companyData != null)
                            {
                                SessionMaster.Current.Logo = _companyData.Logo;
                                SessionMaster.Current.StripLogo = _companyData.LogoStrip;
                            }
                        }
                        if (SessionMaster.Current.Logo ==null)
                        {
                            SessionMaster.Current.Logo = WebConfigurationManager.AppSettings["FilePath"]+"/Content/themes/admin/layout/img/logo.png";
                        }


                        if (SessionMaster.Current.StripLogo == null)
                        {
                            SessionMaster.Current.StripLogo = WebConfigurationManager.AppSettings["FilePath"] + "/Content/themes/admin/layout/img/strip.jpg";
                        }





                        SetupFormsAuthTicket(model.EmailAddress, true, userData);
                        if (String.IsNullOrEmpty(model.ReturnUrl))
                        {

                            if (_data.IsMainUser==true)
                            {
                                //Company
                                //return RedirectToAction("Index", "Company");
                                //by sagar

                                return RedirectToAction("NewRepair", "Repair");
                            }
                            else
                            {
                                int x = Convert.ToInt32(_data.Role);
                                wmpRoleMaster tempRoleObj = _this.wmpRoleMasters.Where(s => s.RoleID == x).FirstOrDefault();

                                if (tempRoleObj.Role == "Finance")
                                {
                                    return RedirectToAction("PaymentHistory", "Company");
                                }

                                 return RedirectToAction("NewRepair", "Repair");
                                //return RedirectToAction("MyProfile", "Account");
                            }

                        }
                        else
                        {
                            return Redirect(model.ReturnUrl);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("LogOnError", "The user name or password provided is incorrect.");
                    }
                }
                else
                {
                    ModelState.AddModelError("LogOnError", "The user name or password provided is incorrect.");
                }
            }
            return View(model);
        }


        public ActionResult RedirectToSomewhere()
        {
            return RedirectToAction("Login", "Home");
        }



        public ActionResult UserTrack()
        {
           
            //wmpUserTrack trck = new wmpUserTrack { Type = "Logout", TimeStamp = DateTime.Now, Uid = Convert.ToInt32(SessionMaster.Current.LoginId) };


            wmpUserTrack trck = _this.wmpUserTracks.Where(s => s.Uid == SessionMaster.Current.LoginId && s.LogoutTime == null).OrderByDescending(s => s.Id).FirstOrDefault();
            if (trck != null)
            {
                trck.LogoutTime = DateTime.UtcNow;

                // _this.wmpUserTracks.Add(trck);
                _this.SaveChanges();
            }

            SessionMaster.Current.LoginId = 0;
            SessionMaster.Current.OwnerID = 0;

            if (Request.IsAuthenticated)
            {

                FormsAuthentication.SignOut();
                Session.Abandon();
                return RedirectToAction("Login", "Home");
            }
            return View();
        }

        public ActionResult Logout()
        {
         
            SessionMaster.Current.LoginId = 0;
            SessionMaster.Current.OwnerID = 0;

            if (Request.IsAuthenticated)
            {

                FormsAuthentication.SignOut();
                Session.Abandon();
                return RedirectToAction("Login", "Home");
            }
            return View();
        }

        private void SetupFormsAuthTicket(string userName, bool persistanceFlag, string userData)
        {
           
           
            var authTicket = new FormsAuthenticationTicket(1, //version
                                userName, // user name
                                DateTime.Now,             //creation
                                DateTime.Now.AddMinutes(30), //Expiration
                                persistanceFlag, //Persistent
                                userData);

            var encTicket = FormsAuthentication.Encrypt(authTicket);
            Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));

        }

        //public ActionResult ForgotPassword(userEmail model)
        //{
        //    return View(model);
        //}
        [HttpPost]
        public JsonResult ForgotPassword(userEmail model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (mSendMail(model))
                    {
                        return Json("Please check your email.");
                    }
                    else
                    {
                        return Json("email address does not exist.");
                    }
                    //return Json("email address does not exist.");
                }
                else
                {
                    return Json("The Email Address field is required.");
                }
            }
            catch (Exception)
            {
                return Json("error in request.");

            }
        }

        private bool mSendMail(userEmail model)
        {
            var _data = _this.wmpUserMasters.FirstOrDefault(c => c.UserName == model.EmailAddress);
           

            if (_data != null)
            {
                var compnyData = _this.wmpCompanyMasters.FirstOrDefault(s => s.OwnerId == _data.OwnerID);

                wmpResetPassword resetModel = new wmpResetPassword();
              
                var _resetPasswordData = _this.wmpResetPasswords.FirstOrDefault(c => c.UserID == _data.UserID);
                string _guidCode = System.Guid.NewGuid().ToString();
                if (_resetPasswordData == null)
                {
                    resetModel.UserID = _data.UserID;
                    resetModel.TimeStamp = DateTime.Now.AddMinutes(30);
                    resetModel.GUID = _guidCode;
                    _this.wmpResetPasswords.Add(resetModel);
                }
                else
                {
                    _resetPasswordData.TimeStamp = DateTime.Now.AddMinutes(30);
                    _resetPasswordData.GUID = _guidCode;
                    _this.Entry(_resetPasswordData).State = EntityState.Modified;
                }
                _this.SaveChanges();
                LinkMode vm = new LinkMode() {
                    Link = WebConfigurationManager.AppSettings["Websitelink"] + "Home/ResetPassword/" + _guidCode,
                    FirstName = _data.FirstName,
                    LastName = _data.LastName,
                    Logo = compnyData.Logo,
                    CompanyName = compnyData.CompanyName
                };
                try
                {
                    string html = RazorViewToString.RenderRazorViewToString(this, "~/Views/emailTemplate/ResetPassword.cshtml", vm);

                    var MailHelper = new MailHelper
                    {
                        Sender = ConfigurationManager.AppSettings["EmailFromAddress"], //email.Sender,

                        //Sender = "reset@watchserve.com", //email.Sender,

                        Recipient = _data.UserName,
                        //RecipientCC = null,
                        RecipientCC = ConfigurationManager.AppSettings["ResetPasswordEmailCC"], //email.Sender,
                        Subject = "WATCHSERVE-Reset Password",
                        Body = html
                    };
                    MailHelper.Send();
                }
                catch (Exception ex)
                {
                    return false;
                }


                return true;
            }
            else
            {
                return false;
            }

        }
        public ActionResult ResetPassword(string id)
        {
            var _checkLink = _this.wmpResetPasswords.FirstOrDefault(c => c.GUID == id);
            
            if (_checkLink == null)
            {
                ViewBag.linkexpired = true;
            }
            else
            {
                if (DateTime.Now > _checkLink.TimeStamp)
                {
                    ViewBag.linkexpired = true;
                }
                else
                {
                    Session["GUID"] = _checkLink.GUID;
                    ViewBag.linkexpired = false;
                }
            }
            return View();
        }

        [HttpPost]
        public ActionResult ResetPassword(LoginUserModel model)
        {
            if (Session["GUID"] != null)
            {
                string _guid = Session["GUID"].ToString();
                var _checkLink = _this.wmpResetPasswords.FirstOrDefault(c => c.GUID == _guid);
                var _userData = _this.wmpUserMasters.FirstOrDefault(c => c.UserID == _checkLink.UserID);
                if (_userData.UserName == model.EmailAddress)
                {
                    _userData.Password = Helper.ComputeHash(model.Password, "SHA512", null);
                    _this.Entry(_userData).State = EntityState.Modified;
                    _this.SaveChanges();
                    ModelState.AddModelError("LogOnError", "Password changed successfully.");
                }
                else
                {
                    ModelState.AddModelError("LogOnError", "The email provided is incorrect.");
                }
                ViewBag.linkexpired = false;
                //  var _userData=_this.wmpUserMasters.FirstOrDefault(c=>c.UserName
            }
            return View(model);
        }


        public JsonResult ChangePasswordManually(string newPassword, string CompanyId, string UserId)
        {
            long UserIdForPasswordReset = 0;
            if (CompanyId != null && CompanyId != "0")
            {
                int x = Convert.ToInt32(CompanyId);
                wmpCompanyMaster obj = _this.wmpCompanyMasters.Where(s => s.CompanyID == x).FirstOrDefault();
                UserIdForPasswordReset = obj.OwnerId;
            }
            else if (UserId != null && UserId != "0")
            {
                int x = Convert.ToInt32(UserId);

                var tempUser = _this.wmpEmployees.Where(s => s.Id == x);
               // var TempUserMAster=_this.wmpUserMasters.Where(s=>s.UserID==)

                UserIdForPasswordReset = (long)tempUser.FirstOrDefault().UserID;
            }
            else
            {
                UserIdForPasswordReset = SessionMaster.Current.LoginId;
            }


            var _userData = _this.wmpUserMasters.FirstOrDefault(c => c.UserID == UserIdForPasswordReset);
            // if (_userData.UserName == model.EmailAddress)
            //{
            _userData.Password = Helper.ComputeHash(newPassword, "SHA512", null);
            _this.Entry(_userData).State = EntityState.Modified;
            _this.SaveChanges();
            ModelState.AddModelError("LogOnError", "Password changed successfully.");



            var _LoginuserData = _this.wmpUserMasters.FirstOrDefault(c => c.UserID == SessionMaster.Current.LoginId);
            int RoleTempId = 0;
            if (_LoginuserData.Role == "admin")
            {
                RoleTempId = 1;
            }
            else
            {
                RoleTempId = Convert.ToInt32(_LoginuserData.Role);
            }
             

            var _LoginUserRole = _this.wmpRoleMasters.FirstOrDefault(s => s.RoleID == RoleTempId);

            PasswordChangedByOther vm = new PasswordChangedByOther()
            {

                FirstName = _userData.FirstName,
                LastName = _userData.LastName,

                ByName = _LoginuserData.FirstName+" "+ _LoginuserData.LastName,
                ByRole = _LoginUserRole.Role,
                NewPassword = newPassword
            };
            try
            {
                string html = RazorViewToString.RenderRazorViewToString(this, "~/Views/emailTemplate/ResetPasswordbyOther.cshtml", vm);

                var MailHelper = new MailHelper
                {
                    Sender = ConfigurationManager.AppSettings["EmailFromAddress"], //email.Sender,
                    Recipient = _userData.UserName,
                    RecipientCC = null,
                    Subject = "WATCHSERVE - Password Changed",
                    Body = html
                };
                MailHelper.Send();
            }
            catch (Exception ex)
            {
                return Json("failed");
            }



            //}
            //else
            //{
            //    ModelState.AddModelError("LogOnError", "The email provided is incorrect.");
            //}
            // ViewBag.linkexpired = false;
            //  var _userData=_this.wmpUserMasters.FirstOrDefault(c=>c.UserName
            return Json("success");
        }

        [Authorize]
        public ActionResult Dashboard()
        {
            //var _d= User.Identity.Name;
            return View();
        }

       // [CustomAuthorize(RolesConfigKey = "RolesConfigKey")]
        public ActionResult About(string page)
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult AddMainAccount()
        {

            wmpUserMaster vm = new wmpUserMaster()
            {
                UserName= "richard@carlstone.com",
                Password= "richard2016",
                FirstName= "richard",
                LastName= "gordon"
            };

            if (_this.wmpUserMasters.Any(c => c.UserName == vm.UserName))
            {
                ViewBag.IsEdit = false;
                ViewBag.Error = "username already exists with this email address (" + vm.UserName + ") !";
                return View(vm);
                //
            }

            if (ModelState.IsValid)
            {
                try
                {
                    vm.Password = Helper.ComputeHash(vm.Password, "SHA512", null);

                    using (TransactionScope scope = new TransactionScope())
                    {

                        wmpUserMaster usr = new wmpUserMaster
                        {
                            UserName = vm.UserName,
                            FirstName = vm.FirstName,
                            LastName = vm.LastName,
                            Password = vm.Password,
                            IsActive = true,
                            IsMainUser = true,
                            OwnerID=1,
                            UpdateBy=1
                            
                        };

                        _this.wmpUserMasters.Add(usr);
                        _this.SaveChanges();
                                                                                     

                        wmpRoleRelationship relationship = new wmpRoleRelationship
                        {
                            OwnerID = usr.OwnerID,
                            RoleID = 1,
                            UserID = usr.UserID
                        };

                        _this.wmpRoleRelationships.Add(relationship);
                        _this.SaveChanges();

                        scope.Complete();

                        //send mail 

                      
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.IsEdit = false;
                    ViewBag.Error = ex.Message;
                }
            }
            else
            {
                ViewBag.IsEdit = false;
            }

            return View();
        }
    }
}
