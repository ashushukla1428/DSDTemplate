using System;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Transactions;
using System.Web.Configuration;
using System.Web.Mvc;
using Wempe.CommonClasses;
using Wempe.Models;

namespace Wempe.Controllers
{
    public class CompanyProfileController : Controller
    {
        //
        // GET: /CompanyProfile/
        dbWempeEntities db = new dbWempeEntities();
        public ActionResult Index()

        {
            ViewBag.Countries = new SelectList(db.wmpCountries.Where(c => c.IsActive == true).OrderBy(c => c.country).Select(c => new { c.Id, c.country }), "Id", "country", 253);

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
                                        //RoleID =int(usr.Role),

                                        LogoStripPath = com.LogoStrip != null && com.LogoStrip != "" ? com.LogoStrip : "/Content/themes/admin/layout/img/strip.jpg",



                                        IsActive = (bool)usr.IsActive,
                                        UserID = usr.UserID,
                                        UserEmail = usr.UserName,
                                        FirstName = usr.FirstName,
                                        LastName = usr.LastName
                                      

                                        , CityId=com.CityId
                                        , StateId=com.StateId
                                        , CountryId=com.CountryId
                                        , ZipCode=com.ZipCode

                                    }).FirstOrDefault();

            var x = db.wmpUserMasters.Where(s => s.UserID == SessionMaster.Current.LoginId);

            SessionMaster.Current.StripLogo = rec.LogoStripPath;

            rec.RoleID = Convert.ToInt32(x.FirstOrDefault().Role);

            return View(rec);

         
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(CompanyUserModel_Old vm)
        {
            if (vm.LogoStrip != null)
            {
                System.Drawing.Image img = System.Drawing.Image.FromStream(vm.LogoStrip.InputStream);
                int height = img.Height;
                int width = img.Width;
                if (height > 50 || width > 80)
                {
                    ViewBag.IsEdit = false;
                    TempData["Error"] = "Strip image must not exceed for Height 50px and for Width 80 px.";
                    //return View(vm);
                    return RedirectToAction("Index");
                }
            }


            //vm.LogoStrip.C

            long t1 = SessionMaster.Current.OwnerID;
            long t2 = SessionMaster.Current.LoginId;
            //var x = db.wmpCompanyPackages.Where(s => s.CompanyId == vm.CompanyID).FirstOrDefault();

            //vm.Payment = x.Payment;
            //vm.UserLimit = x.UserLimit.ToString();
            //vm.CityId = 0;
            //vm.StateId = 0;
            //vm.CountryId = 0;

            ModelState.Remove("Password");
            ModelState.Remove("ConfirmPassword");


            if (db.wmpUserMasters.Any(c => c.UserName == vm.UserEmail && c.UserID != vm.UserID))
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

                        com.CityId = vm.CityId;
                        com.StateId = vm.StateId;
                        com.CountryId = vm.CountryId;
                        com.ZipCode = vm.ZipCode;

                        if (vm.Logo != null && vm.Logo.ContentLength > 0)
                        {
                            //Delete old file
                            if (com.Logo != null && com.Logo != "")
                            {
                                if (Directory.Exists(Server.MapPath(com.Logo)))
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

                            string file = Path.Combine(path, Path.GetFileName(vm.Logo.FileName)); //path + "/" + vm.Logo.FileName;
                            vm.Logo.SaveAs(file);

                            com.Logo = WebConfigurationManager.AppSettings["FilePath"] + "/Logo/" + com.CompanyID + "/" + vm.Logo.FileName;
                        }


                        // logo strip -- 24 Jan 2017


                        if (vm.LogoStrip != null && vm.LogoStrip.ContentLength > 0)
                        {
                            //Delete old strip file
                            if (com.LogoStrip != null && com.LogoStrip != "")
                            {
                                if (Directory.Exists(Server.MapPath(com.LogoStrip)))
                                {
                                    System.IO.File.Delete(Server.MapPath(com.LogoStrip));
                                }
                            }

                            //Saving new strip
                            string path = Server.MapPath("~/Logo/" + vm.CompanyID);

                            if (!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                            }

                            string file = Path.Combine(path, Path.GetFileName(vm.LogoStrip.FileName)); //path + "/" + vm.Logo.FileName;
                            vm.LogoStrip.SaveAs(file);

                            com.LogoStrip = WebConfigurationManager.AppSettings["FilePath"] + "/Logo/" + com.CompanyID + "/" + vm.LogoStrip.FileName;
                        }

                        // End logo strip -- 24 Jan 2017






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

                        //ViewBag.Error = "Company profile updated succesfully";
                        TempData["Error"] = "Company profile updated succesfully";
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.IsEdit = true;
                    //ViewBag.Error = "Some error occur";
                    TempData["Error"] = "Some error occur";
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
