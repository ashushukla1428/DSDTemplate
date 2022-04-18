using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Globalization;
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
    [CustomAuthorize()]
    public class UserController : Controller
    {
        dbWempeEntities db = new dbWempeEntities();
        public ActionResult UserTrack()
        {
            return View();
        }
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public JsonResult getStates(int Id)
        {
            try
            {
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
                return Json(db.wmpSampleCities.Where(c => c.IsActive == true && c.StateId == Id).OrderBy(c => c.city).Select(c => new { c.Id, c.city }), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new Result { Status = false, Message = ex.Message });
            }
        }
        public ActionResult Create()
        {
            var x = db.wmpCompanyMasters.Where(s => s.OwnerId == SessionMaster.Current.OwnerID);
            if (x.Any())
            {
                var pckae = db.wmpCompanyPackages.Where(s => s.CompanyId == x.FirstOrDefault().CompanyID).FirstOrDefault();
                if (pckae != null)
                {
                    int totalUsers = db.wmpUserMasters.Where(s => s.OwnerID == SessionMaster.Current.OwnerID && s.Role != "Company").Count();
                    ViewBag.CompanyUserMessage = "Current total users: "+totalUsers+", Maximum users limit: " + pckae.UserLimit;
                }
            }
            ViewBag.Countries = new SelectList(db.wmpCountries.Where(c => c.IsActive == true).OrderBy(c => c.country).Select(c => new { c.Id, c.country }), "Id", "country", 253);
            ViewBag.IsEdit = false;
            var _pageData = db.wmpRoleMasters.Where(c => c.IsActive == true && c.IsMainRole == false || c.IsActive == true && c.IsMainRole==null).Select(c => new { c.RoleID, c.Role });
            SelectList list = new SelectList(_pageData, "RoleID", "Role");
            ViewBag.Roles = list.OrderBy(s=>s.Text);
            CompanyEmployeeModel vm = new CompanyEmployeeModel();
            vm.Password = Helper.CreateRandomPassword(8);
            vm.ConfirmPassword = vm.Password;
            vm.IsActive = true;
            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CompanyEmployeeModel vm)
        {
            ViewBag.Countries = new SelectList(db.wmpCountries.Where(c => c.IsActive == true).OrderBy(c => c.country).Select(c => new { c.Id, c.country }), "Id", "country", 253);
            var _pageData = db.wmpRoleMasters.Where(c => c.IsActive == true && c.IsMainRole == false || c.IsActive == true && c.IsMainRole == null).Select(c => new { c.RoleID, c.Role });
            SelectList list = new SelectList(_pageData, "RoleID", "Role");
            ViewBag.Roles = list.OrderBy(s => s.Text);
            var x = db.wmpCompanyMasters.Where(s => s.OwnerId == SessionMaster.Current.OwnerID);
            if (x.Any())
            {
                var pckae = db.wmpCompanyPackages.Where(s => s.CompanyId == x.FirstOrDefault().CompanyID).FirstOrDefault();
                if (pckae != null)
                {
                    long totalUsers = db.wmpUserMasters.Where(s => s.OwnerID == SessionMaster.Current.OwnerID && s.Role!= "Company").Count();
                    if (totalUsers ==  pckae.UserLimit)
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
                            FirstName = vm.FirstName==null?vm.employeeFirstName:vm.FirstName,
                            LastName = vm.LastName==null?vm.employeeLastName:vm.LastName,
                            Password = vm.ConfirmPassword,
                            OwnerID= SessionMaster.Current.OwnerID,
                            Role = vm.employeeRoleID.ToString(),
                            IsActive = vm.IsActive,
                            IsMainUser = SessionMaster.Current.IsMainUser
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
                            OwnerID = SessionMaster.Current.OwnerID,
                            UpdateBy = SessionMaster.Current.LoginId,
                            LastUpdate = DateTime.Now,
                            CountryId = vm.CountryId,
                            StateId = vm.StateId,
                            CityId = vm.CityId
                            , ZipCode = vm.ZipCode
                        };
                        db.wmpEmployees.Add(emp);
                        db.SaveChanges();
                        if (vm.Image != null && vm.Image.ContentLength > 0)
                        {
                            string path = Server.MapPath("~/UploadFiles/" + emp.OwnerID+"/Employee");
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
            return RedirectToAction("index");
        }
        [HttpPost]
        public JsonResult getList(SearchFilters model)
        {
            try
            {
                var _items = db.Database.SqlQuery<UserModel>("USP_GetUsers @p0, @p1, @p2, @p3, @p4,@p5,@p6", model.Name == null ? "" : model.Name, model.pageNo, Convert.ToInt32(MainSetting.pageSize), model.sortColumn, model.sortOrder,SessionMaster.Current.OwnerID, model.UserType);
                return Json(_items, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }



        //[HttpPost]
        //public JsonResult getListForSuperAdmin(SearchFilters model)
        //{
        //    try
        //    {
        //        long? CompanyOwnerId = 0;

        //        if (TempData["CompanyOwnerId"] != null)
        //        {
        //            CompanyOwnerId = Convert.ToInt64(TempData["CompanyOwnerId"]);
        //            TempData["CompanyOwnerId"] = CompanyOwnerId;
        //        }
        //        else
        //        {
        //            var _cmpny = db.wmpCompanyMasters.Where(s => s.CompanyID == model.CompanyId).FirstOrDefault();
        //            TempData["CompanyOwnerId"] = _cmpny.OwnerId;
        //            CompanyOwnerId = _cmpny.OwnerId;
        //        }
                 
        //        var _items = db.Database.SqlQuery<UserModel>("USP_GetUsers @p0, @p1, @p2, @p3, @p4,@p5,@p6", model.Name == null ? "" : model.Name, model.pageNo, Convert.ToInt32(MainSetting.pageSize), model.sortColumn, model.sortOrder, CompanyOwnerId, model.UserType);
        //        return Json(_items, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(ex.Message);
        //    }
        //}



        [HttpPost]
        public JsonResult getUserTrackList(SearchFilters model)
        {
            try
            {
                var _items = db.Database.SqlQuery<UserTrackModel>("USP_GetUserTrack @p0, @p1, @p2, @p3, @p4,@p5,@p6", model.Name == null ? "" : model.Name, model.pageNo, Convert.ToInt32(MainSetting.pageSize), model.sortColumn, model.sortOrder, SessionMaster.Current.OwnerID, model.UserType).ToList();

                foreach (var item in _items)
                {
                    if (item.LoginTime != null)
                    {
                        item.LoginTime = item.LoginTime.Value.ToLocalTime();
                        item.LoginTimeString = item.LoginTime.Value.ToLocalTime().ToString();
                    }
                    if (item.LogoutTime != null)
                    {
                        item.LogoutTime = item.LogoutTime.Value.ToLocalTime();
                        item.LogoutTimeString = item.LogoutTime.Value.ToLocalTime().ToString();
                    }
                }

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
                                         ZipCode=com.ZipCode,
                                        IsActive = (bool)usr.IsActive,
                                        UserID = usr.UserID,
                                        UserEmail = usr.UserName,
                                        FirstName = usr.FirstName,
                                        LastName = usr.LastName,
                                        CountryId= com.CountryId,
                                        StateId=com.StateId,
                                        CityId=com.CityId
                                    }).FirstOrDefault();
            ViewBag.States = new SelectList(db.wmpStates.Where(c => c.IsActive == true && c.CountryId == rec.CountryId).OrderBy(c => c.stateFullName).Select(c => new { c.Id, c.stateFullName }), "Id", "stateFullName", rec.StateId);
            return View(rec);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CompanyEmployeeModel vm)
        {
            ModelState.Remove("Password");
            ModelState.Remove("ConfirmPassword");
            if (db.wmpUserMasters.Any(c => c.UserName == vm.UserEmail && c.UserID!=vm.UserID))
            {
                ViewBag.IsEdit = false;
                var _pageData2 = db.wmpRoleMasters.Where(c => c.IsActive == true && c.OwnerID == SessionMaster.Current.OwnerID).Select(c => new { c.RoleID, c.Role });
                SelectList list2 = new SelectList(_pageData2, "RoleID", "Role");
                ViewBag.Roles = list2.OrderBy(s => s.Text);
                ViewBag.Error = "This email address (" + vm.UserEmail + ") is used by another user !";
                return View(vm);
            }
            var _pageData = db.wmpRoleMasters.Where(c => c.IsActive == true && c.OwnerID == SessionMaster.Current.OwnerID).Select(c => new { c.RoleID, c.Role });
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
                        com.employee = vm.employeeFirstName+" "+ vm.employeeLastName;
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
        #region Company Role and rights 
        public ActionResult Rights()
        {

            var data = new PagedData<wmpRoleMaster>();

            data.Data = db.wmpRoleMasters.OrderByDescending(p => p.Role).Where(c => c.OwnerID == SessionMaster.Current.OwnerID).Take(500).ToList();
            data.NumberOfPages = Convert.ToInt32(Math.Ceiling((double)db.wmpRoleMasters.Where(c => c.OwnerID == SessionMaster.Current.OwnerID).Count() / 500));
            return View(data);
        }
        [HttpGet]
        public ActionResult CompanyRoles(int page)
        {
            var data = new PagedData<wmpRoleMaster>();
            data.Data = db.wmpRoleMasters.OrderByDescending(p => p.Role).Where(c => c.OwnerID == SessionMaster.Current.OwnerID).ToList();
            data.NumberOfPages = Convert.ToInt32(Math.Ceiling((double)db.wmpRoleMasters.Where(c => c.OwnerID == SessionMaster.Current.OwnerID).Count() / 500));
            data.CurrentPage = page;

            return PartialView(data);
        }
        /// <summary>
        /// A method to populate a TreeView with directories, subdirectories, etc
        /// </summary>
        /// <param name="dir">The path of the directory</param>
        /// <param name="node">The "master" node, to populate</param>
        public void PopulateTree(int RoleID, JsTreeModel node, bool OnlyForCompanyUsers)
        {
            if (node.children == null)
            {
                node.children = new List<JsTreeModel>();
            }
            // get the information of the directory
            //.Where(c=>c.OnlyForCompanyUsers==false)
            var _rolesList = db.wmpMVCAuthenticationRights.Where(c => c.OnlyForCompanyUsers == OnlyForCompanyUsers && c.IsActive == true)
                   .GroupBy(p => p.ControllerName)
                   .Select(g => new { name = g.Key, count = g.Count() });
            int count = 0;
            // loop through each subdirectory
            foreach (var item in _rolesList)
            {

                count = _rolesList.Count();
                // create a new node
                JsTreeModel t = new JsTreeModel();
                t.attr = new JsTreeAttribute();
                t.attr.id = "0";
                t.state = "closed";

                t.data = item.name.ToString();
                //if (count > 0)
                //{
                //    t.data = item.name.ToString()+ " (" + count.ToString() + ")";
                //}
                //else
                //{
                //    t.data = item.name.ToString();
                //}
                // populate the new node recursively
                PopulateTreeSubItem(item.name, RoleID, t, OnlyForCompanyUsers);
                node.children.Add(t); // add the node to the "master" node
            }

        }
        public void PopulateTreeSubItem(string parentID, int roleID, JsTreeModel node, bool OnlyForCompanyUsers)
        {
            if (node.children == null)
            {
                node.children = new List<JsTreeModel>();
            }
            // get the information of the directory
            // DirectoryInfo directory = new DirectoryInfo("");
            // loop through each subdirectory
            var _rolesList = db.Database.SqlQuery<RightsRoleWiseModel>("USP_RightsRoleWise @p0,@p1,@p2", roleID, parentID,OnlyForCompanyUsers);
            // var _rolesList = _allCategories.Where(c => c.ParentID == parentID).OrderBy(c => c.SortOrder);

            foreach (var item in _rolesList)
            {
                //  count = _rolesList.Count();
                // create a new node
                JsTreeModel t = new JsTreeModel();
                t.attr = new JsTreeAttribute();

                t.attr.id = item.ActionID.ToString();
                if (item.Access)
                {
                    t.attr.Class = "jstree-checked";
                }
                // t.attr.selected = item.Access;
                t.state = "disabled";
                //t.disable = true;
                t.data = item.RightFor.ToString();

                // populate the new node recursively
                PopulateTreeSubItem(item.ActionName, roleID, t,OnlyForCompanyUsers);
                node.children.Add(t); // add the node to the "master" node
            }

        }
        [HttpPost]
        public JsonResult getCompanyPages(int RoleID)
        {

            JsTreeModel rootNode = new JsTreeModel();
            rootNode.attr = new JsTreeAttribute();
            rootNode.data = "Company Section";
            rootNode.attr.id = "0";
            //string rootPath = Request.MapPath("/Controllers");
            //rootNode.attr.id = rootPath;
            PopulateTree(RoleID, rootNode, true);
            return Json(rootNode);

        }
        [HttpPost]
        public JsonResult getRightsRoleWise(int RoleID)
        {
            try
            {
                var _items = db.Database.SqlQuery<RightsRoleWiseModel>("USP_RightsRoleWise @p0", RoleID);

                return Json(_items, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(ex.Message);
            }
        }
        [HttpGet]
        public JsonResult EditRole(int id)
        {
            var _data = db.wmpRoleMasters.Find(id);
            return Json(new { _data.RoleID, _data.Role, _data.IsActive }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public JsonResult AddRole(RoleMaster _model)
        {
            try
            {
                TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;

                string TempName = textInfo.ToTitleCase(_model.Role); //War And Peace
                _model.Role = TempName;

                // _model.Role = _model.Role.ToUpperInvariant();

                wmpRoleMaster model = new wmpRoleMaster();

                model.UpdateBy = SessionMaster.Current.LoginId;
                model.LastUpdate = DateTime.Now;
                model.OwnerID = SessionMaster.Current.OwnerID;

                model.RoleID = _model.RoleID;
                model.Role = _model.Role;
                model.IsActive = _model.IsActive;
                if (ModelState.IsValid)
                {


                    if (model.RoleID == 0)
                    {
                        if (db.wmpRoleMasters.Any(c => c.Role == model.Role && c.OwnerID == SessionMaster.Current.OwnerID))
                        {
                            return Json(new Result { Status = false, Message = Messages.recordAlreadyExists }, JsonRequestBehavior.AllowGet);
                        }
                        db.wmpRoleMasters.Add(model);
                    }
                    else
                    {
                        if (db.wmpRoleMasters.Any(c => c.Role == model.Role && c.OwnerID == SessionMaster.Current.OwnerID && c.RoleID != model.RoleID))
                        {
                            return Json(new Result { Status = false, Message = Messages.recordAlreadyExists }, JsonRequestBehavior.AllowGet);
                        }

                        db.Entry(model).State = EntityState.Modified;
                    }
                    db.SaveChanges();
                    db.Database.ExecuteSqlCommand("USP_SetRights @p0,@p1", model.RoleID, _model.ActionsID);

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
        #endregion 


        public ActionResult ManageUsers()
        {
            return View();
        }
    }
}
