using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Wempe.CommonClasses;

namespace Wempe.Controllers
{
    public class CategoryController : Controller
    {
        dbWempeEntities db = new dbWempeEntities();
        //
        // GET: /Category/
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult Add(wmpCategoryMaster model)
        {
            try
            {
                model.LastUpdate = DateTime.Now;
                model.UpdateBy = SessionMaster.Current.LoginId;
                model.OwnerID = SessionMaster.Current.OwnerID;
                if (ModelState.IsValid)
                {
                    if (model.CategoryID == 0)
                    {
                        
                        if (db.wmpCategoryMasters.Any(c => c.CategoryName == model.CategoryName && c.OwnerID == model.OwnerID))
                        {
                            return Json(new Result { Status = false, Message = Messages.recordAlreadyExists }, JsonRequestBehavior.AllowGet);
                        }
                        db.wmpCategoryMasters.Add(model);
                    }
                    else
                    {
                        if (db.wmpCategoryMasters.Any(c => c.CategoryName == model.CategoryName && c.CategoryID != model.CategoryID && c.OwnerID == model.OwnerID))
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
        //Delete
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult UploadFile()
        {
            string _imgname = string.Empty;
            if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
            {
                var pic = System.Web.HttpContext.Current.Request.Files["MyImages"];
                if (pic.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(pic.FileName);
                    var _ext = Path.GetExtension(pic.FileName);

                    _imgname = Guid.NewGuid().ToString();
                    var _comPath = Server.MapPath("/Upload/cat_") + fileName ;
                    _imgname = "cat_" + fileName ;

                    ViewBag.Msg = _comPath;
                    var path = _comPath;

                    // Saving Image in Original Mode
                    pic.SaveAs(path);

                    // resizing image
                    MemoryStream ms = new MemoryStream();
                    WebImage img = new WebImage(_comPath);

                    if (img.Width > 200)
                        img.Resize(200, 200);
                    img.Save(_comPath);
                    // end resize
                }
            }
            return Json(Convert.ToString(_imgname), JsonRequestBehavior.AllowGet);
        }


        #region Menu in tree view
        [HttpGet]
        public JsonResult getCategoryDetail(Int64 ID)
        {


            var _item = db.wmpCategoryMasters.Where(c => c.CategoryID == ID).FirstOrDefault();
            return Json(_item, JsonRequestBehavior.AllowGet);


        }
        [HttpPost]
        public JsonResult getMenus()
        {


            JsTreeModel rootNode = new JsTreeModel();
            rootNode.attr = new JsTreeAttribute();
            rootNode.data = "Root";
            rootNode.attr.id = "0";
            //string rootPath = Request.MapPath("/Controllers");
            //rootNode.attr.id = rootPath;
            PopulateTree(0, rootNode);
            return Json(rootNode);


        }
        /// <summary>
        /// A method to populate a TreeView with directories, subdirectories, etc
        /// </summary>
        /// <param name="dir">The path of the directory</param>
        /// <param name="node">The "master" node, to populate</param>
        public void PopulateTree(Int64 parentID, JsTreeModel node)
        {
            if (node.children == null)
            {
                node.children = new List<JsTreeModel>();
            }


            var _list = db.wmpCategoryMasters.Where(c => c.parentID == parentID).OrderBy(c => c.CategoryIndex);

            int count = 0;
            // loop through each subdirectory
            foreach (var item in _list)
            {

                count = _list.Count();
                // create a new node
                JsTreeModel t = new JsTreeModel();
                t.attr = new JsTreeAttribute();
                t.attr.id =item.CategoryID.ToString();
                t.state = "disabled";

                t.data = item.CategoryName.ToString();
                //if (count > 0)
                //{
                //    t.data = item.name.ToString()+ " (" + count.ToString() + ")";
                //}
                //else
                //{
                //    t.data = item.name.ToString();
                //}
                // populate the new node recursively
                PopulateTree(item.CategoryID, t);
                node.children.Add(t); // add the node to the "master" node
            }

        }
        #endregion
    }
}
