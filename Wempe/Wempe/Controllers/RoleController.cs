using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Wempe.CommonClasses;
using Wempe.Models;

namespace Wempe.Controllers
{
    [CustomAuthorize()]
    public class RoleController : Controller
    {
        dbWempeEntities db = new dbWempeEntities();
        [CustomAuthorize()]
        public ActionResult Index()
        {
            var data = new PagedData<wmpRoleMaster>();
            data.Data = db.wmpRoleMasters.OrderBy(p => p.Role ).Where(c=>c.OwnerID==SessionMaster.Current.OwnerID).Take(500).ToList();
            data.NumberOfPages = Convert.ToInt32(Math.Ceiling((double)db.wmpRoleMasters.Where(c => c.OwnerID == SessionMaster.Current.OwnerID).Count() / 500));
            return View(data);
        }
        [HttpGet]
        public JsonResult PageList(int page, string IsActive)
        {
            var data = new PagedData<wmpRoleMaster>();
            if (IsActive == "1")
            {
                data.Data = db.wmpRoleMasters.OrderBy(p => p.Role).Where(c => c.OwnerID == SessionMaster.Current.OwnerID && c.IsActive == true).Skip(500 * (page - 1)).Take(500).ToList();
            }
            else
            {
                data.Data = db.wmpRoleMasters.OrderBy(p => p.Role).Where(c => c.OwnerID == SessionMaster.Current.OwnerID).Skip(500 * (page - 1)).Take(500).ToList();
            }
            return  Json(data, JsonRequestBehavior.AllowGet);
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
            var _rolesList = db.wmpMVCAuthenticationRights.Where(c => c.OnlyForCompanyUsers == OnlyForCompanyUsers && c.IsActive==true)
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
                t.attr.Class = "jstree-checked sgrRolePopUp";
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
            var _rolesList = db.Database.SqlQuery<RightsRoleWiseModel>("USP_RightsRoleWise @p0,@p1,@p2", roleID, parentID, OnlyForCompanyUsers);
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
                    t.attr.Class = "jstree-checked sgrRolePopUp";
                }
               // t.attr.selected = item.Access;
                t.state = "disabled";
                //t.disable = true;
                t.data = item.RightFor.ToString();
                // populate the new node recursively
                PopulateTreeSubItem(item.ActionName, roleID, t, OnlyForCompanyUsers);
                node.children.Add(t); // add the node to the "master" node
            }
        }
        [HttpPost]
        public JsonResult getAdminPages(int RoleID)
        {
            JsTreeModel rootNode = new JsTreeModel();
            rootNode.attr = new JsTreeAttribute();
            rootNode.data = "Admin Section";
            rootNode.attr.id = "0";
            //string rootPath = Request.MapPath("/Controllers");
            //rootNode.attr.id = rootPath;
            PopulateTree(RoleID, rootNode, false);
            return Json(rootNode);
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
        public JsonResult Edit(int id)
        {
            var _data = db.wmpRoleMasters.Find(id);
            return Json(new { _data.RoleID, _data.Role, _data.IsActive }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Add(RoleMaster _model)
        {
            try
            {
                TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                string TempName = textInfo.ToTitleCase(_model.Role); //War And Peace
                _model.Role = TempName;
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
                        if (db.wmpRoleMasters.Any(c => c.Role == model.Role && c.OwnerID == SessionMaster.Current.OwnerID && c.RoleID!=model.RoleID))
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
    }
}
