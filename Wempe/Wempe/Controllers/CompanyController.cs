using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using Wempe.CommonClasses;
using Wempe.Models;

namespace Wempe.Controllers
{
  // [CustomAuthorize()]
    public class CompanyController : Controller
    {
        dbWempeEntities db = new dbWempeEntities();

        public ActionResult Index()
        {
            
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


        public JsonResult searchRepair(int PageNo, string Type,string StartDate, string EndDate, string sortColumn, string sortOrder)
        {
            try
            {
                var _items = db.Database.SqlQuery<wmpRepairforSearch>("USP_GetRepairForCompanyDashboard @p0, @p1, @p2, @p3,@p4,@p5,@p6", PageNo, Convert.ToInt32(MainSetting.pageSize), Type, StartDate, EndDate,SessionMaster.Current.OwnerID, sortColumn, sortOrder);
                return Json(_items, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(new Result { Status = false, Message = ex.Message });
            }
        }

        public ActionResult Create()
        {
            ViewBag.Countries = new SelectList(db.wmpCountries.Where(c => c.IsActive == true).OrderBy(c => c.country).Select(c => new { c.Id, c.country }), "Id", "country", 253);
            ViewBag.IsEdit = false;
            //var _pageData = db.wmpRoleMasters.Where(c => c.IsActive == true && c.OwnerID == SessionMaster.Current.OwnerID).Select(c => new { c.RoleID, c.Role });
            //SelectList list = new SelectList(_pageData, "RoleID", "Role");
            //ViewBag.Roles = list;
            CompanyUserModel vm = new CompanyUserModel();
            vm.Password = Helper.CreateRandomPassword(8);
            vm.ConfirmPassword = vm.Password;
            vm.IsActive = true;
            return View(vm);
        }


        public ActionResult Dashboard()
        {
            try
            {
                //var _items = new GetToalAndPendingRepairList
                //{
                //    TotalRepairs = db.Database.SqlQuery<ReportSummary>("GetNoOfRepairs").ToList(),
                //    PendingRepairs = db.Database.SqlQuery<PendingReportSummary>("GetNoOfPendingRepairs").ToList()
                //};
                //ViewBag.SystemOverViewNumerofTotalRepairs = _items.TotalRepairs.Sum(s => s.NumberofRepairs);
                //ViewBag.SystemOverViewNumerofPendingRepairs = _items.PendingRepairs.Count();
                //ViewBag.Years = Enumerable.Range(2003, (DateTime.Now.AddYears(1).Year - 2003)).Reverse();
                return View();
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CompanyUserModel vm)
        {
           

            ViewBag.Countries = new SelectList(db.wmpCountries.Where(c => c.IsActive == true).OrderBy(c => c.country).Select(c => new { c.Id, c.country }), "Id", "country", 253);
            //var _pageData = db.wmpRoleMasters.Where(c => c.IsActive == true && c.OwnerID == SessionMaster.Current.OwnerID).Select(c => new { c.RoleID, c.Role });
            //SelectList list = new SelectList(_pageData, "RoleID", "Role");
            //ViewBag.Roles = list;
            if (db.wmpUserMasters.Any(c => c.UserName == vm.UserEmail))
            {
                ViewBag.IsEdit = false;
                ViewBag.Error = "company already exists with this (" + vm.UserEmail + ") username !";
                return View(vm);
                //
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
                            FirstName = vm.FirstName,
                            LastName = vm.LastName,
                            Password = vm.ConfirmPassword,

                            Role = "2",
                            IsActive = vm.IsActive,
                            IsMainUser = false
                        };

                        db.wmpUserMasters.Add(usr);
                        db.SaveChanges();
                        
                        usr.OwnerID = usr.UserID;//Self owner has owner id
                        db.Entry(usr).State = EntityState.Modified;
                        db.SaveChanges();

                        wmpCompanyMaster com = new wmpCompanyMaster
                        {
                            CompanyName = vm.CompanyName,
                            ContactNumber = vm.PhoneNumber,
                            EmailAddress = vm.EmailAddress,
                            WebSite = vm.Website,
                            Address = vm.Address1,
                            Address1 = vm.Address2,

                            IsActive = vm.IsActive,
                            OwnerId = Convert.ToInt32(usr.OwnerID),
                            UpdateBy = SessionMaster.Current.LoginId,
                            LastUpdate = DateTime.Now,

                            CountryId=vm.CountryId,
                            StateId=vm.StateId,
                            CityId =vm.CityId

                            , ZipCode=vm.ZipCode
                        };

                        db.wmpCompanyMasters.Add(com);
                        db.SaveChanges();

                        if (vm.Logo != null && vm.Logo.ContentLength > 0)
                        {
                            string path = Server.MapPath("~/Logo/" + com.CompanyID);

                            if (!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                            }

                            string file = Path.Combine(path, Path.GetFileName(vm.Logo.FileName.Replace(" ","")));
                            vm.Logo.SaveAs(file);

                            wmpCompanyMaster upCom = db.wmpCompanyMasters.Where(p => p.CompanyID == com.CompanyID).FirstOrDefault();
                            upCom.Logo =  WebConfigurationManager.AppSettings["FilePath"]+"/Logo/" + com.CompanyID + "/" + vm.Logo.FileName.Replace(" ", "");
                            db.Entry(upCom).State = EntityState.Modified;
                            db.SaveChanges();
                        }




                        // 24 Jan 2017 -- for logo strip

                        if (vm.LogoStrip != null && vm.LogoStrip.ContentLength > 0)
                        {
                            string path = Server.MapPath("~/Logo/" + com.CompanyID);

                            if (!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                            }

                            string file = Path.Combine(path, Path.GetFileName(vm.LogoStrip.FileName.Replace(" ", "")));
                            vm.LogoStrip.SaveAs(file);

                            wmpCompanyMaster upCom = db.wmpCompanyMasters.Where(p => p.CompanyID == com.CompanyID).FirstOrDefault();
                            upCom.LogoStrip = WebConfigurationManager.AppSettings["FilePath"] + "/Logo/" + com.CompanyID + "/" + vm.LogoStrip.FileName.Replace(" ", "");
                            db.Entry(upCom).State = EntityState.Modified;
                            db.SaveChanges();
                        }


                        // End 24 Jan 2017 -- for logo strip

                        wmpRoleRelationship relationship = new wmpRoleRelationship
                        {
                            OwnerID=usr.OwnerID,
                            RoleID=2,
                            UserID=usr.UserID
                        };

                        db.wmpRoleRelationships.Add(relationship);
                        db.SaveChanges();


                        wmpCompanyMaster upCom2 = db.wmpCompanyMasters.Where(p => p.CompanyID == com.CompanyID).FirstOrDefault();

                        wmpEmployee emp = new wmpEmployee { Address1 = vm.Address1, Address2 = vm.Address2, CityId = vm.CityId, CountryId = vm.CountryId, EmailAddress = vm.EmailAddress, employeeFirstName = vm.FirstName, employeeLastName = vm.LastName,
                            IsActive = vm.IsActive,
                            //  OwnerId = Convert.ToInt32(usr.OwnerID),
                            UpdateBy = SessionMaster.Current.LoginId,
                            LastUpdate = DateTime.Now,
                            OwnerID = Convert.ToInt32(usr.OwnerID), PhoneNumber = vm.PhoneNumber, UserID = usr.UserID,
                            employee = vm.FirstName + " " + vm.LastName, RoleID = Convert.ToInt32(relationship.RoleID), Image = upCom2.Logo,

                            //CountryId = vm.CountryId,
                            StateId = vm.StateId, 
                           // CityId = vm.CityId
                        };
                        db.wmpEmployees.Add(emp);
                        db.SaveChanges();
                        
                        //wmpRoleMaster _roleModel = new wmpRoleMaster() { Role = "admin", OwnerID = usr.UserID, UpdateBy = SessionMaster.Current.OwnerID, IsActive = true, LastUpdate = DateTime.Now };
                        //db.wmpRoleMasters.Add(_roleModel);
                        //db.SaveChanges();


                        wmpCompanyPackage _companyPackageModel = new wmpCompanyPackage() { CompanyId = com.CompanyID, CreatedBy = SessionMaster.Current.LoginId, CreatedDate = DateTime.Now, OwnerId = Convert.ToInt32(usr.OwnerID), PackageRenewalDate = vm.PackageRenewalDate, PackageStartDate =vm.PackageStartDate, PaymentMode = vm.PaymentMode, PaymentStatus = vm.PaymentStatus, UserLimit = Convert.ToInt64(vm.UserLimit), PackagePeriod=vm.PackagePeriod, Payment=vm.Payment, IsActive=true };
                        db.wmpCompanyPackages.Add(_companyPackageModel);
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
                                Subject = "Company login detail",
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
            return RedirectToAction("index");
        }

        [HttpPost]
        public JsonResult getList(SearchFilters model)
        {
            try
            {
                var _items = db.Database.SqlQuery<CompanyModel>("USP_GetCompanies @p0, @p1, @p2, @p3, @p4, @p5", model.Name == null ? "" : model.Name, model.pageNo, Convert.ToInt32(MainSetting.pageSize), model.sortColumn, model.sortOrder, model.ActiveOrAllCheck);

                return Json(_items, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

       

        [HttpGet]
        public ActionResult Edit(int id)
        {
            ViewBag.Countries = new SelectList(db.wmpCountries.Where(c => c.IsActive == true).OrderBy(c => c.country).Select(c => new { c.Id, c.country }), "Id", "country", 253);
            ViewBag.IsEdit = true;

            //var _pageData = db.wmpRoleMasters.Where(c => c.IsActive == true && c.OwnerID == SessionMaster.Current.OwnerID).Select(c => new { c.RoleID, c.Role });
            //SelectList list = new SelectList(_pageData, "RoleID", "Role");
            //ViewBag.Roles = list;

            CompanyUserModel rec = (from com in db.wmpCompanyMasters
                                    join usr in db.wmpUserMasters on com.OwnerId equals usr.OwnerID

                                    join role in db.wmpRoleRelationships on usr.UserID equals role.UserID

                                    into temp
                                    from j in temp.DefaultIfEmpty()


                                    join package in db.wmpCompanyPackages on com.CompanyID equals package.CompanyId

                                    into tempPackage
                                    from j2 in tempPackage.DefaultIfEmpty()




                                    where com.CompanyID == id
                                    select new CompanyUserModel
                                    {
                                        CompanyID = com.CompanyID,
                                        CompanyName = com.CompanyName,
                                        Address1 = com.Address,
                                        Address2 = com.Address1,
                                        PhoneNumber = com.ContactNumber,
                                        EmailAddress = com.EmailAddress,
                                        Website = com.WebSite,
                                        LogoPath = com.Logo != null && com.Logo != "" ? com.Logo : "/images/nologo.jpg",

                                         LogoStripPath = com.LogoStrip != null && com.LogoStrip != "" ? com.LogoStrip : "/Content/themes/admin/layout/img/strip.jpg",

                                        //RoleID = (int)j.RoleID,
                                        RoleID = j.RoleID == null ? 0 : (int)j.RoleID,
                                        IsActive = (bool)usr.IsActive,
                                        UserID = usr.UserID,
                                        UserEmail = usr.UserName,
                                        FirstName = usr.FirstName,
                                        LastName = usr.LastName,
                                        
                                        CountryId=com.CountryId,
                                        StateId=com.StateId,
                                        CityId=com.CityId,

                                         ZipCode=com.ZipCode,
                                        // new columns

                                        PackageStartDate=j2.PackageStartDate.ToString(),
                                        PackageRenewalDate=j2.PackageRenewalDate.ToString(),
                                        PaymentMode=j2.PaymentMode,
                                        PaymentStatus=j2.PaymentStatus,
                                        UserLimit=j2.UserLimit.ToString(),

                                         Payment=j2.Payment,
                                          PackagePeriod=j2.PackagePeriod

                                    }).FirstOrDefault();


            ViewBag.States = new SelectList(db.wmpStates.Where(c => c.IsActive == true && c.CountryId==rec.CountryId).OrderBy(c => c.stateFullName).Select(c => new { c.Id, c.stateFullName }), "Id", "stateFullName", rec.StateId);

            return View(rec);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CompanyUserModel vm)
        {
            ModelState.Remove("Password");
            ModelState.Remove("ConfirmPassword");
            ModelState.Remove("Payment");
            ModelState.Remove("PackageStartDate");
            ModelState.Remove("UserLimit");
            //var _pageData = db.wmpRoleMasters.Where(c => c.IsActive == true && c.OwnerID == SessionMaster.Current.OwnerID).Select(c => new { c.RoleID, c.Role });
            //SelectList list = new SelectList(_pageData, "RoleID", "Role");
            //ViewBag.Roles = list;
            ViewBag.IsEdit = true;
            if (ModelState.IsValid)
            {
                try
                {
                    using (TransactionScope scope = new TransactionScope())
                    {
                        wmpCompanyMaster com = db.wmpCompanyMasters.Where(p => p.CompanyID == vm.CompanyID).FirstOrDefault();
                        com.CompanyName = vm.CompanyName;
                        com.Address = vm.Address1;
                        com.Address1 = vm.Address2;
                        com.ContactNumber = vm.PhoneNumber;
                        com.WebSite = vm.Website;
                        com.EmailAddress = vm.EmailAddress;
                        com.LastUpdate = DateTime.Now;
                        com.UpdateBy = SessionMaster.Current.LoginId;
                        com.IsActive = vm.IsActive;

                        com.CountryId = vm.CountryId;
                        com.StateId = vm.StateId;
                        com.CityId = vm.CityId;
                        com.ZipCode = vm.ZipCode;

                        if (vm.Logo != null && vm.Logo.ContentLength > 0)
                        {
                            //Delete old file
                            if (com.Logo != null && com.Logo != "")
                            {
                                if (!Directory.Exists(Server.MapPath(com.Logo)))
                                {
                                    System.IO.File.Delete(Server.MapPath(com.Logo));
                                }
                            }                         

                            //Saving new logo
                            string path = Server.MapPath("~/Logo/" + vm.CompanyID);

                            if (!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                            }

                            string file = Path.Combine(path, Path.GetFileName(vm.Logo.FileName));
                            vm.Logo.SaveAs(file);

                            com.Logo = WebConfigurationManager.AppSettings["FilePath"] + "/Logo/" + com.CompanyID + "/" + vm.Logo.FileName;
                        }





                        // Logo strip -- 24 Jan 2017

                        if (vm.LogoStrip != null && vm.LogoStrip.ContentLength > 0)
                        {
                            //Delete old file
                            if (com.LogoStrip != null && com.LogoStrip != "")
                            {
                                if (!Directory.Exists(Server.MapPath(com.LogoStrip)))
                                {
                                    System.IO.File.Delete(Server.MapPath(com.LogoStrip));
                                }
                            }

                            //Saving new logo
                            string path = Server.MapPath("~/Logo/" + vm.CompanyID);

                            if (!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                            }

                            string file = Path.Combine(path, Path.GetFileName(vm.LogoStrip.FileName));
                            vm.LogoStrip.SaveAs(file);

                            com.LogoStrip = WebConfigurationManager.AppSettings["FilePath"] + "/Logo/" + com.CompanyID + "/" + vm.LogoStrip.FileName;
                        }

                        // End Logo Strip -- 24 Jan 2017


                        db.Entry(com).State = EntityState.Modified;
                        db.SaveChanges();


                        wmpUserMaster usr = db.wmpUserMasters.Where(p => p.UserID == vm.UserID).FirstOrDefault();
                        usr.UserName = vm.UserEmail;
                        usr.FirstName = vm.FirstName;
                        usr.IsMainUser = false;
                        usr.LastName = vm.LastName;
                        usr.LastUpdate = DateTime.Now;
                        usr.UpdateBy = SessionMaster.Current.LoginId;
                        usr.IsActive = vm.IsActive;
                        db.Entry(usr).State = EntityState.Modified;
                        db.SaveChanges();




                        wmpCompanyPackage package = db.wmpCompanyPackages.Where(p => p.CompanyId == vm.CompanyID).FirstOrDefault();


                        //if (package.PackageStartDate != vm.PackageStartDate && package.PackageRenewalDate != vm.PackageRenewalDate)
                        //{
                        //    wmpCompanyPackage _companyPackageModel = new wmpCompanyPackage() { CompanyId = com.CompanyID, CreatedBy = SessionMaster.Current.LoginId, CreatedDate = DateTime.Now, OwnerId = SessionMaster.Current.OwnerID, PackageRenewalDate = vm.PackageRenewalDate, PackageStartDate = vm.PackageStartDate, PaymentMode = vm.PaymentMode, PaymentStatus = vm.PaymentStatus, UserLimit = Convert.ToInt64(vm.UserLimit), PackagePeriod = vm.PackagePeriod, Payment = vm.Payment };
                        //    db.wmpCompanyPackages.Add(_companyPackageModel);
                        //    db.SaveChanges();



                        //}
                        //else
                        //{
                            package.PackageRenewalDate = vm.PackageRenewalDate;
                            package.PackagePeriod = vm.PackagePeriod;
                            package.Payment = vm.Payment;
                            package.PackageStartDate = vm.PackageStartDate;
                            package.PaymentMode = vm.PaymentMode;
                            package.PaymentStatus = vm.PaymentStatus;
                        //package.OwnerId=vm.
                            package.UserLimit = Convert.ToInt64(vm.UserLimit);
                            //package.UpdateBy = SessionMaster.Current.LoginId;
                            //package.IsActive = vm.IsActive;
                            db.Entry(package).State = EntityState.Modified;
                            db.SaveChanges();
                       // }




                        //wmpRoleRelationship relationship = db.wmpRoleRelationships.Where(c => c.UserID == vm.UserID).FirstOrDefault();
                        //relationship.RoleID = vm.RoleID;
                        //db.Entry(relationship).State = EntityState.Modified; 
                        //db.SaveChanges();

                        scope.Complete();
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Countries = new SelectList(db.wmpCountries.Where(c => c.IsActive == true).OrderBy(c => c.country).Select(c => new { c.Id, c.country }), "Id", "country", 253);
                    ViewBag.IsEdit = true;
                    ViewBag.Error = "Some error occur";
                    
                    return View(vm);
                }
            }
            else
            {
                ViewBag.Countries = new SelectList(db.wmpCountries.Where(c => c.IsActive == true).OrderBy(c => c.country).Select(c => new { c.Id, c.country }), "Id", "country", 253);
                ViewBag.IsEdit = true;
                return View(vm);
            }
            return RedirectToAction("index");
        }
        
        [HttpPost]
        public JsonResult Add(wmpAppraiserMaster model)
        {
            try
            {
                model.LastUpdate = DateTime.Now;
                model.UpdateBy = SessionMaster.Current.LoginId;
                if (ModelState.IsValid)
                {
                    if (model.AppraiserID == 0)
                    {
                        if (db.wmpAppraiserMasters.Any(c => c.AppraiserTitle == model.AppraiserTitle))
                        {
                            return Json(new Result { Status = false, Message = Messages.recordAlreadyExists }, JsonRequestBehavior.AllowGet);
                        }
                        db.wmpAppraiserMasters.Add(model);
                    }
                    else
                    {
                        if (db.wmpAppraiserMasters.Any(c => c.AppraiserTitle == model.AppraiserTitle && c.AppraiserID != model.AppraiserID))
                        {
                            return Json(new Result { Status = false, Message = Messages.recordAlreadyExists }, JsonRequestBehavior.AllowGet);
                        }
                        db.Entry(model).State = EntityState.Modified;
                    }
                    db.SaveChanges();
                    return Json(new Result { Status = true, Message = Messages.recordSaved }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    string _error = string.Empty;
                    foreach (ModelState modelState in ViewData.ModelState.Values)
                    {
                        foreach (ModelError error in modelState.Errors)
                        {
                            _error = _error + error;
                        }
                    }
                    return Json(new Result { Status = false, Message = _error }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new Result { Status = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            //  return Json(new Result { Status = false, Message = _error }, JsonRequestBehavior.AllowGet); 
            try
            {
                var data = db.wmpAppraiserMasters.Find(id);
                db.wmpAppraiserMasters.Remove(data);
                db.SaveChanges();
                return Json(new Result { Status = true, Message = Messages.recordDeleted }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(new Result { Status = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public virtual ActionResult UploadFile()
        {
            HttpPostedFileBase myFile = Request.Files["MyFile"];
            bool isUploaded = false;
            string message = "File upload failed";

            if (myFile != null && myFile.ContentLength != 0)
            {
                string pathForSaving = Server.MapPath("~/Content/CompanyLogo");
                if (this.CreateFolderIfNeeded(pathForSaving))
                {
                    try
                    {
                        myFile.SaveAs(Path.Combine(pathForSaving, myFile.FileName));
                        isUploaded = true;
                        message = "File uploaded successfully!";
                    }
                    catch (Exception ex)
                    {
                        message = string.Format("File upload failed: {0}", ex.Message);
                    }
                }
            }
            return Json(new { isUploaded = isUploaded, message = message }, "text/html");
        }



        /// <summary>
        /// Creates the folder if needed.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        private bool CreateFolderIfNeeded(string path)
        {
            bool result = true;
            if (!Directory.Exists(path))
            {
                try
                {
                    Directory.CreateDirectory(path);
                }
                catch (Exception)
                {
                    /*TODO: You must process this exception.*/
                    result = false;
                }
            }
            return result;
        }

        [HttpPost]
        public void SetTheme(string themeName)
        {
            SessionMaster.Current.themeName = themeName;
        }
        // ashu2 -- for company users section


        public class wmpPaymentHistory :  PageingModel
        {
            public long Id { get; set; }

            [Required]
            public string PackageStartDate { get; set; }
            public string PackageRenewalDate { get; set; }

            [Required]
            [Phone]
            [DataType(DataType.PhoneNumber)]
            [StringLength(4)]
            public string Payment { get; set; }
            public string PaymentStatus { get; set; }
            public string PackagePeriod { get; set; }

            [Required]
            [Phone]
            [DataType(DataType.PhoneNumber)]
            [StringLength(4)]
            public string UserLimit { get; set; }

           // public bool IsActive { get; set; }
            //public string PrimaryPhone { get; set; }
        }

        [HttpPost]
        public JsonResult ViewPaymentHistory(PaymentHistory model)
        {
            try
            {
                if (model.CompanyId == 0)
                {
                    model.CompanyId = db.wmpCompanyMasters.Where(s => s.CompanyID == model.CompanyId).FirstOrDefault().CompanyID;
                }

                model.sortColumn = "firstName";
                model.sortOrder = "desc";
                //var _items = db.Database.SqlQuery<wmpCustomerforSearch>("USP_SearchCustomer @p0, @p1, @p2, @p3, @p4,@p5,@p6", model.ColName, model.ColValue, model.pageNo, Convert.ToInt32(MainSetting.pageSize), model.sortColumn, model.sortOrder, SessionMaster.Current.OwnerID);
                var _items = db.Database.SqlQuery<wmpPaymentHistory>("USP_ViewPaymentHistory @p0, @p1, @p2, @p3, @p4", model.CompanyId, model.pageNo, Convert.ToInt32(MainSetting.pageSize), model.sortColumn, model.sortOrder);
                return Json(_items, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(new Result { Status = false, Message = ex.Message });
            }
        }



        [HttpPost]
        public JsonResult ViewPaymentHistoryForFinanceUser(PaymentHistory model)
        {
            try
            {
                //if (model.CompanyId == 0)
                //{
                //    model.CompanyId = db.wmpCompanyMasters.Where(s => s.CompanyID == model.CompanyId).FirstOrDefault().CompanyID;
                //}


                wmpCompanyMaster obj = db.wmpCompanyMasters.Where(s => s.OwnerId == SessionMaster.Current.OwnerID).FirstOrDefault();
                model.CompanyId = obj.CompanyID;
                model.sortColumn = "firstName";
                model.sortOrder = "desc";
                //var _items = db.Database.SqlQuery<wmpCustomerforSearch>("USP_SearchCustomer @p0, @p1, @p2, @p3, @p4,@p5,@p6", model.ColName, model.ColValue, model.pageNo, Convert.ToInt32(MainSetting.pageSize), model.sortColumn, model.sortOrder, SessionMaster.Current.OwnerID);
                var _items = db.Database.SqlQuery<wmpPaymentHistory>("USP_ViewPaymentHistory @p0, @p1, @p2, @p3, @p4", model.CompanyId, model.pageNo, Convert.ToInt32(MainSetting.pageSize), model.sortColumn, model.sortOrder);
                return Json(_items, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(new Result { Status = false, Message = ex.Message });
            }
        }


        // [ValidateAntiForgeryToken]
        [HttpPost]
        public JsonResult SaveCompanyPayment(PaymentHistoryModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var _model = db.wmpCompanyMasters.Where(c => c.CompanyID == model.CompanyId).FirstOrDefault();
                    if (model.Id != 0)
                    {
                        wmpCompanyPackage obj = db.wmpCompanyPackages.Where(s => s.Id == model.Id && s.IsActive == true).FirstOrDefault();

                        obj.UserLimit = model.UserLimit;
                        obj.PackageStartDate = model.PackageStartDate;
                        obj.PackageRenewalDate = model.PackageRenewalDate;
                        obj.Payment = model.Payment;
                        obj.OwnerId = _model.OwnerId;
                        obj.PaymentMode = model.PaymentMode;
                        obj.PaymentStatus = model.PaymentStatus;
                        obj.PackagePeriod = model.PackagePeriod;
                        db.SaveChanges();
                        return Json("Success", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        //if (model.CompanyId == 0)
                        //{
                        //    model.CompanyId = db.wmpCompanyMasters.Where(s => s.OwnerId == SessionMaster.Current.OwnerID).FirstOrDefault().CompanyID;
                        //}
                        wmpCompanyPackage obj = db.wmpCompanyPackages.Where(s => s.CompanyId == model.CompanyId && s.IsActive == true).FirstOrDefault();
                        if (obj != null)
                        {
                            obj.IsActive = false;
                            db.SaveChanges();
                        }

                        wmpCompanyPackage _companyPackageModel = new wmpCompanyPackage() { CompanyId = model.CompanyId, CreatedBy = SessionMaster.Current.LoginId, CreatedDate = DateTime.Now, OwnerId = _model.OwnerId, PackageRenewalDate = model.PackageRenewalDate, PackageStartDate = model.PackageStartDate, PaymentMode = model.PaymentMode, PaymentStatus = model.PaymentStatus, UserLimit = Convert.ToInt64(model.UserLimit), PackagePeriod = model.PackagePeriod, Payment = model.Payment, IsActive = true };
                        db.wmpCompanyPackages.Add(_companyPackageModel);
                        db.SaveChanges();

                        return Json("Success", JsonRequestBehavior.AllowGet);
                    }
                }

                catch (Exception ex)
                {

                    return Json(new Result { Status = false, Message = ex.Message });
                }
            }
            else
            {
                return Json(new Result { Status = false, Message = "Validation" });
            }
        }


        public ActionResult PaymentHistory()
        {
            return View();
        }

        [HttpPost]
        public JsonResult editPaymentHistory(PaymentHistoryModel model)
        {
            try
            {
                wmpCompanyPackage obj = db.wmpCompanyPackages.Where(s => s.Id==model.Id).FirstOrDefault();
                if (obj != null)
                {
                    if (obj.IsActive == false)
                    {
                        return Json("NotAllowed", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {

                        return Json(obj, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(new Result { Status = false, Message = "" });

            }
            catch (Exception ex)
            {

                return Json(new Result { Status = false, Message = ex.Message });
            }
        }


    }
}
