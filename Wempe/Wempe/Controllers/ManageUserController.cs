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
    [CustomAuthorize()]
    public class ManageUserController : Controller
    {
        dbWempeEntities db = new dbWempeEntities();
        //
        // GET: /ManageUser/

        public ActionResult Create()
        {

            long CompanyOwnerId =Convert.ToInt64(TempData["CompanyOwnerId"]);

            TempData["CompanyOwnerId"] = CompanyOwnerId;

            var x = db.wmpCompanyMasters.Where(s => s.OwnerId == CompanyOwnerId);
            if (x.Any())
            {
                var pckae = db.wmpCompanyPackages.Where(s => s.CompanyId == x.FirstOrDefault().CompanyID).FirstOrDefault();
                if (pckae != null)
                {
                    int totalUsers = db.wmpUserMasters.Where(s => s.OwnerID == CompanyOwnerId && s.Role != "Company").Count();
                    ViewBag.CompanyUserMessage = "Current total users: " + totalUsers + ", Maximum users limit: " + pckae.UserLimit;
                }
            }
            ViewBag.Countries = new SelectList(db.wmpCountries.Where(c => c.IsActive == true).OrderBy(c => c.country).Select(c => new { c.Id, c.country }), "Id", "country", 253);
            ViewBag.IsEdit = false;
            var _pageData = db.wmpRoleMasters.Where(c => c.IsActive == true && c.IsMainRole == false || c.IsActive == true && c.IsMainRole == null).Select(c => new { c.RoleID, c.Role });
            SelectList list = new SelectList(_pageData, "RoleID", "Role");
            ViewBag.Roles = list.OrderBy(s => s.Text);
            CompanyEmployeeModel vm = new CompanyEmployeeModel();
            vm.Password = Helper.CreateRandomPassword(8);
            vm.ConfirmPassword = vm.Password;
            vm.IsActive = true;
            return View(vm);
        }



        [HttpGet]
        public ActionResult Edit(int id)
        {
            long CompanyOwnerId = Convert.ToInt64(TempData["CompanyOwnerId"]);

            TempData["CompanyOwnerId"] = CompanyOwnerId;


            ViewBag.Countries = new SelectList(db.wmpCountries.Where(c => c.IsActive == true).OrderBy(c => c.country).Select(c => new { c.Id, c.country }), "Id", "country", 253);
            ViewBag.IsEdit = true;
            var _pageData = db.wmpRoleMasters.Where(c => c.IsActive == true && c.IsMainRole == false || c.IsActive == true && c.IsMainRole == null).Select(c => new { c.RoleID, c.Role });
            SelectList list = new SelectList(_pageData, "RoleID", "Role");
            ViewBag.Roles = list.OrderBy(s => s.Text);
            CompanyEmployeeModel rec = (from com in db.wmpEmployees
                                        join usr in db.wmpUserMasters on com.UserID equals usr.UserID
                                        join role in db.wmpRoleRelationships on usr.UserID equals role.UserID
                                        where com.Id == id
                                        select new CompanyEmployeeModel
                                        {
                                            // CompanyID = com.CompanyID,
                                            employeeFirstName = com.employeeFirstName,
                                            employeeLastName = com.employeeLastName,
                                            //employee = com.employee,
                                            employeeAddress1 = com.Address1,
                                            employeeAddress2 = com.Address2,
                                            employeePhoneNumber = com.PhoneNumber,
                                            employeeEmailAddress = com.EmailAddress,
                                            //Website = com.WebSite,
                                            ImagePath = com.Image != null && com.Image != "" ? com.Image : "/images/nologo.jpg",
                                            employeeRoleID = (int)role.RoleID,
                                            ZipCode = com.ZipCode,
                                            IsActive = (bool)usr.IsActive,
                                            UserID = usr.UserID,
                                            UserEmail = usr.UserName,
                                            FirstName = usr.FirstName,
                                            LastName = usr.LastName,
                                            CountryId = com.CountryId,
                                            StateId = com.StateId,
                                            CityId = com.CityId
                                        }).FirstOrDefault();
            ViewBag.States = new SelectList(db.wmpStates.Where(c => c.IsActive == true && c.CountryId == rec.CountryId).OrderBy(c => c.stateFullName).Select(c => new { c.Id, c.stateFullName }), "Id", "stateFullName", rec.StateId);
            return View(rec);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CompanyEmployeeModel vm)
        {

            long CompanyOwnerId = Convert.ToInt64(TempData["CompanyOwnerId"]);

            TempData["CompanyOwnerId"] = CompanyOwnerId;


            ViewBag.Countries = new SelectList(db.wmpCountries.Where(c => c.IsActive == true).OrderBy(c => c.country).Select(c => new { c.Id, c.country }), "Id", "country", 253);
            var _pageData = db.wmpRoleMasters.Where(c => c.IsActive == true && c.IsMainRole == false || c.IsActive == true && c.IsMainRole == null).Select(c => new { c.RoleID, c.Role });
            SelectList list = new SelectList(_pageData, "RoleID", "Role");
            ViewBag.Roles = list.OrderBy(s => s.Text);
            var x = db.wmpCompanyMasters.Where(s => s.OwnerId == CompanyOwnerId);
            if (x.Any())
            {
                var pckae = db.wmpCompanyPackages.Where(s => s.CompanyId == x.FirstOrDefault().CompanyID).FirstOrDefault();
                if (pckae != null)
                {
                    long totalUsers = db.wmpUserMasters.Where(s => s.OwnerID == CompanyOwnerId && s.Role != "Company").Count();
                    if (totalUsers == pckae.UserLimit)
                    {
                        ViewBag.IsEdit = false;
                        ViewBag.Error = "User limits exceed!";
                        return View(vm);
                    }
                }
            }
            if (vm.Password != vm.ConfirmPassword)
            {
                ViewBag.IsEdit = false;
                ViewBag.Error = "Password Mismatch!";
                return View(vm);
            }
            if (db.wmpUserMasters.Any(c => c.UserName == vm.UserEmail))
            {
                ViewBag.IsEdit = false;
                ViewBag.Error = "username already exists with this email address (" + vm.UserEmail + ") !";
                return View(vm);
            }
            if (ModelState.IsValid)
            {
                try
                {
                    vm.ConfirmPassword = Helper.ComputeHash(vm.Password, "SHA512", null);
                    using (TransactionScope scope = new TransactionScope())
                    {
                        wmpUserMaster usr = new wmpUserMaster
                        {
                            UserName = vm.UserEmail,
                            FirstName = vm.FirstName == null ? vm.employeeFirstName : vm.FirstName,
                            LastName = vm.LastName == null ? vm.employeeLastName : vm.LastName,
                            Password = vm.ConfirmPassword,
                            OwnerID = CompanyOwnerId,
                            UpdateBy= SessionMaster.Current.OwnerID,
                            Role = vm.employeeRoleID.ToString(),
                            IsActive = vm.IsActive,
                            //IsMainUser = SessionMaster.Current.IsMainUser
                            IsMainUser=false
                        };
                        db.wmpUserMasters.Add(usr);
                        db.SaveChanges();
                        wmpEmployee emp = new wmpEmployee
                        {
                            employeeFirstName = vm.employeeFirstName,
                            employeeLastName = vm.employeeLastName,
                            employee = vm.employeeFirstName + " " + vm.employeeLastName,
                            PhoneNumber = vm.employeePhoneNumber,
                            EmailAddress = vm.employeeEmailAddress,
                            //WebSite = vm.Website,
                            Address1 = vm.employeeAddress1,
                            Address2 = vm.employeeAddress2,
                            UserID = usr.UserID,
                            RoleID = vm.employeeRoleID,
                            IsActive = vm.IsActive,
                            OwnerID = CompanyOwnerId,
                            UpdateBy = SessionMaster.Current.LoginId,
                            LastUpdate = DateTime.Now,
                            CountryId = vm.CountryId,
                            StateId = vm.StateId,
                            CityId = vm.CityId
                            ,
                            ZipCode = vm.ZipCode
                        };
                        db.wmpEmployees.Add(emp);
                        db.SaveChanges();
                        if (vm.Image != null && vm.Image.ContentLength > 0)
                        {
                            string path = Server.MapPath("~/UploadFiles/" + emp.OwnerID + "/Employee");
                            if (!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                            }
                            string file = Path.Combine(path, Path.GetFileName(vm.Image.FileName));
                            vm.Image.SaveAs(file);
                            wmpEmployee upEmp = db.wmpEmployees.Where(p => p.UserID == emp.UserID).FirstOrDefault();
                            upEmp.Image = WebConfigurationManager.AppSettings["FilePath"] + "/UploadFiles/" + emp.OwnerID + "/Employee/" + vm.Image.FileName;
                            db.Entry(upEmp).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        wmpRoleRelationship relationship = new wmpRoleRelationship
                        {
                            OwnerID = usr.OwnerID,
                            RoleID = vm.employeeRoleID,
                            UserID = usr.UserID
                        };
                        db.wmpRoleRelationships.Add(relationship);
                        db.SaveChanges();
                        scope.Complete();
                        //send mail 
                        try
                        {
                            string html = RazorViewToString.RenderRazorViewToString(this, "~/Views/emailTemplate/subscriptionsEmail.cshtml", vm);
                            var MailHelper = new MailHelper
                            {
                                Sender = ConfigurationManager.AppSettings["EmailFromAddress"], //email.Sender,
                                Recipient = vm.UserEmail,
                                RecipientCC = null,
                                Subject = "User login detail",
                                Body = html
                            };
                            MailHelper.Send();
                        }
                        catch (Exception ex)
                        {
                            ViewBag.IsEdit = false;
                            ViewBag.Error = ex.Message;
                            return View(vm);
                        }
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.IsEdit = false;
                    ViewBag.Error = ex.Message;
                    return View(vm);
                }
            }
            else
            {
                ViewBag.IsEdit = false;
                return View(vm);
            }
            return RedirectToAction("ManageUsers");
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CompanyEmployeeModel vm)
        {

            long CompanyOwnerId = Convert.ToInt64(TempData["CompanyOwnerId"]);

            TempData["CompanyOwnerId"] = CompanyOwnerId;


            ModelState.Remove("Password");
            ModelState.Remove("ConfirmPassword");
            if (db.wmpUserMasters.Any(c => c.UserName == vm.UserEmail && c.UserID != vm.UserID))
            {
                ViewBag.IsEdit = false;
                var _pageData2 = db.wmpRoleMasters.Where(c => c.IsActive == true && c.OwnerID == CompanyOwnerId).Select(c => new { c.RoleID, c.Role });
                SelectList list2 = new SelectList(_pageData2, "RoleID", "Role");
                ViewBag.Roles = list2.OrderBy(s => s.Text);
                ViewBag.Error = "This email address (" + vm.UserEmail + ") is used by another user !";
                return View(vm);
            }
            var _pageData = db.wmpRoleMasters.Where(c => c.IsActive == true && c.OwnerID == CompanyOwnerId).Select(c => new { c.RoleID, c.Role });
            SelectList list = new SelectList(_pageData, "RoleID", "Role");
            ViewBag.Roles = list.OrderBy(s => s.Text);
            ViewBag.IsEdit = true;
            if (ModelState.IsValid)
            {
                try
                {
                    using (TransactionScope scope = new TransactionScope())
                    {
                        wmpEmployee com = db.wmpEmployees.Where(p => p.Id == vm.Id).FirstOrDefault();
                        com.employeeFirstName = vm.employeeFirstName;
                        com.employeeLastName = vm.employeeLastName;
                        com.employee = vm.employeeFirstName + " " + vm.employeeLastName;
                        com.Address1 = vm.employeeAddress1;
                        com.Address2 = vm.employeeAddress2;
                        com.PhoneNumber = vm.employeePhoneNumber;
                        //com.WebSite = vm.Website;
                        com.EmailAddress = vm.employeeEmailAddress;
                        com.LastUpdate = DateTime.Now;
                        com.UpdateBy = SessionMaster.Current.LoginId;
                        com.IsActive = vm.IsActive;
                        com.ZipCode = vm.ZipCode;
                        com.CountryId = vm.CountryId;
                        com.StateId = vm.StateId;
                        com.CityId = vm.CityId;
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
                            //Saving new logo
                            // if (vm.Image != null && vm.Image.ContentLength > 0)
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
                        wmpUserMaster usr = db.wmpUserMasters.Where(p => p.UserID == vm.UserID).FirstOrDefault();
                        usr.UserName = vm.UserEmail;
                        if (vm.FirstName != null)
                        {
                            usr.FirstName = vm.FirstName;
                        }
                        else
                        {
                            usr.FirstName = vm.employeeFirstName;
                        }
                        usr.IsMainUser = false;

                        if (vm.LastName != null)
                        {
                            usr.LastName = vm.LastName;
                        }
                        else
                        {
                            usr.LastName = vm.employeeLastName;
                        }
                        usr.LastUpdate = DateTime.Now;
                        usr.UpdateBy = SessionMaster.Current.LoginId;
                        usr.IsActive = vm.IsActive;
                        db.Entry(usr).State = EntityState.Modified;
                        db.SaveChanges();
                        wmpRoleRelationship relationship = db.wmpRoleRelationships.Where(c => c.UserID == vm.UserID).FirstOrDefault();
                        relationship.RoleID = vm.employeeRoleID;
                        db.Entry(relationship).State = EntityState.Modified;
                        db.SaveChanges();
                        scope.Complete();
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.IsEdit = true;
                    ViewBag.Error = "Some error occur";
                    return View(vm);
                }
            }
            else
            {
                ViewBag.IsEdit = true;
                return View(vm);
            }
            return RedirectToAction("ManageUsers");
        }




        public ActionResult ManageUsers()
        {
            return View();
        }


        [HttpPost]
        public JsonResult getListForSuperAdmin(SearchFilters model)
        {
            try
            {
                long? CompanyOwnerId = 0;

                if (model.CompanyId != null)
                {
                    var _cmpny = db.wmpCompanyMasters.Where(s => s.CompanyID == model.CompanyId).FirstOrDefault();
                    TempData["CompanyOwnerId"] = _cmpny.OwnerId;
                    CompanyOwnerId = _cmpny.OwnerId;


                }
                else
                {

                    CompanyOwnerId = Convert.ToInt64(TempData["CompanyOwnerId"]);
                    TempData["CompanyOwnerId"] = CompanyOwnerId;
                }

                var _items = db.Database.SqlQuery<UserModel>("USP_GetUsers @p0, @p1, @p2, @p3, @p4,@p5,@p6", model.Name == null ? "" : model.Name, model.pageNo, Convert.ToInt32(MainSetting.pageSize), model.sortColumn, model.sortOrder, CompanyOwnerId, model.UserType);
                return Json(_items, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }



        [HttpPost]
        public ActionResult simulate(string UserID)
        {
            long UserId = Convert.ToInt64(UserID);
            var _data = db.wmpEmployees.FirstOrDefault(c => c.Id == UserId);
            var _data2= db.wmpUserMasters.FirstOrDefault(c => c.UserID == _data.UserID);
            //SessionMaster.Current.LoginId = UserId;
            SessionMaster.Current.LoginId = _data2.UserID;

            long ownerId =Convert.ToInt64(_data.OwnerID);
            SessionMaster.Current.OwnerID = ownerId;
            SessionMaster.Current.IsMainUser = (bool)_data2.IsMainUser;
            SessionMaster.Current.Logo = null;
            SessionMaster.Current.StripLogo = null;

            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;

            string TempName = textInfo.ToTitleCase(_data2.FirstName + " " + _data2.LastName); //War And Peace


            wmpRoleRelationship a = db.wmpRoleRelationships.Where(s => s.UserID == _data2.UserID).FirstOrDefault();
            wmpRoleMaster obj = db.wmpRoleMasters.Where(s => s.RoleID == a.RoleID).FirstOrDefault();

            SessionMaster.Current.Name = TempName;

            SessionMaster.Current.Role = obj.Role;



          //  string userData = JsonConvert.SerializeObject(serializeModel);
            if (_data2.IsMainUser == false)
            {
                var _companyData = db.wmpCompanyMasters.Where(c => c.OwnerId == _data.OwnerID).FirstOrDefault();
                if (_companyData != null)
                {
                    SessionMaster.Current.Logo = _companyData.Logo;
                    SessionMaster.Current.StripLogo = _companyData.LogoStrip;
                }
            }
            if (SessionMaster.Current.Logo == null)
            {
                SessionMaster.Current.Logo = WebConfigurationManager.AppSettings["FilePath"] + "/Content/themes/admin/layout/img/logo.png";
            }


            if (SessionMaster.Current.StripLogo == null)
            {
                SessionMaster.Current.StripLogo = WebConfigurationManager.AppSettings["FilePath"] + "/Content/themes/admin/layout/img/strip.jpg";
            }


           // if (Request.IsAuthenticated)
           // {

                //  FormsAuthentication.SignOut();
                // Session.Abandon();
                //Response.Redirect(Request.RawUrl);

                return RedirectToAction("NewRepair", "Repair");
           // }
          //  return View();

        }
    }
}
