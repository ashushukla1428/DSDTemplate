using System;
using System.Collections.Generic;
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
    public class ComapnyProfileController : Controller
    {
        //
        // GET: /ComapnyProfile/
        dbWempeEntities db = new dbWempeEntities();
        public ActionResult Index()
        {
            CompanyUserModel rec = (from com in db.wmpCompanyMasters
                                    join usr in db.wmpUserMasters on com.OwnerId equals SessionMaster.Current.OwnerID
                                    join role in db.wmpRoleRelationships on usr.UserID equals SessionMaster.Current.LoginId
                                   // where com.CompanyID == id
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
                                        RoleID = (int)role.RoleID,

                                        IsActive = (bool)usr.IsActive,
                                        UserID = usr.UserID,
                                        UserEmail = usr.UserName,
                                        FirstName = usr.FirstName,
                                        LastName = usr.LastName
                                      
                                    }).FirstOrDefault();




            return View(rec);

         
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(CompanyUserModel vm)
        {
            ModelState.Remove("Password");
            ModelState.Remove("ConfirmPassword");

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


                        db.Entry(com).State = EntityState.Modified;
                        db.SaveChanges();
                        SessionMaster.Current.Logo = com.Logo;

                        wmpUserMaster usr = db.wmpUserMasters.Where(p => p.UserID == SessionMaster.Current.LoginId).FirstOrDefault();
                        usr.UserName = vm.UserEmail;
                        usr.FirstName = vm.FirstName;
                        usr.IsMainUser = false;
                        usr.LastName = vm.LastName;
                        usr.LastUpdate = DateTime.Now;
                        usr.UpdateBy = SessionMaster.Current.LoginId;
                       
                        db.Entry(usr).State = EntityState.Modified;
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
            return RedirectToAction("index");
        }


    }
}
