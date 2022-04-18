using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wempe.CommonClasses;
using Wempe.Models;

namespace Wempe.Controllers
{
    public class PagesController : Controller
    {
        dbWempeEntities db = new dbWempeEntities();
        int PageSize = 10;
        public ActionResult Index()
        {
            var data = new PagedData<wmpWebsitePage>();
            data.Data = db.wmpWebsitePages.OrderByDescending(p => p.PageName).Take(PageSize).ToList();
            data.NumberOfPages = Convert.ToInt32(Math.Ceiling((double)db.wmpMenuMasters.Count() / PageSize));
            return View(data);
        }
        [HttpGet]
        public ActionResult PageList(int page)
        {
            var data = new PagedData<wmpWebsitePage>();
            data.Data = db.wmpWebsitePages.OrderByDescending(p => p.PageName).Skip(PageSize * (page - 1)).Take(PageSize).ToList();
            data.NumberOfPages = Convert.ToInt32(Math.Ceiling((double)db.wmpMenuMasters.Count() / PageSize));
            data.CurrentPage = page;
            return PartialView(data);
        }
        public ActionResult Create()
        {
            return View();
        }
        public ActionResult Edit(Int64 id)
        {
            var _model = db.wmpWebsitePages.Where(c => c.PageID == id).FirstOrDefault();
            return View(_model);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(wmpWebsitePage model, FormCollection _collection, HttpPostedFileBase txtfile)
        {
            model.LastUpdate = DateTime.Now;
            if (ModelState.IsValid)
            {
                if (txtfile != null)
                {
                    model.PageImage = uploadPostImage(txtfile);
                }
                db.wmpWebsitePages.Add(model);
                db.SaveChanges();
                var hiddenId = _collection["hndCateggory"].Replace("\"", "").Split(',');
                db.Database.ExecuteSqlCommand("USP_resetPagelining @p0", model.PageID);
                foreach (var item in hiddenId)
                {
                    if (item != "" && item != "0")
                    {
                        Int64 _cid = Convert.ToInt64(item);
                        var _model = db.wmpWebsitePagesLinkings.Where(c => c.CatID == _cid && c.PageID == model.PageID).FirstOrDefault();
                        if (_model == null)
                        {
                            wmpWebsitePagesLinking _pageModel = new wmpWebsitePagesLinking()
                            {
                                PageID = model.PageID,
                                CatID = Convert.ToInt64(item),
                                IsActive = true
                            };
                            db.wmpWebsitePagesLinkings.Add(_pageModel);
                            db.SaveChanges();
                        }
                        else
                        {
                            _model.IsActive = true;
                            db.Entry(_model).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                    }
                }
            }
            return View(model);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(wmpWebsitePage model, FormCollection _collection, HttpPostedFileBase txtfile)
        {
            model.LastUpdate = DateTime.Now;
            if (ModelState.IsValid)
            {
                if (txtfile != null)
                {
                    model.PageImage= uploadPostImage(txtfile);
                }
                    db.Entry(model).State = EntityState.Modified;
                    db.SaveChanges();
                    var hiddenId = _collection["hndCateggory"].Replace("\"", "").Split(',');
                    db.Database.ExecuteSqlCommand("USP_resetPagelining @p0", model.PageID);
                    foreach (var item in hiddenId)
                    {
                        if (item != "" && item!="0")
                        {
                            Int64 _cid=Convert.ToInt64(item);
                            var _model = db.wmpWebsitePagesLinkings.Where(c => c.CatID == _cid && c.PageID == model.PageID).FirstOrDefault();
                            if (_model == null)
                            {
                                wmpWebsitePagesLinking _pageModel = new wmpWebsitePagesLinking()
                                {
                                    PageID = model.PageID,
                                    CatID = Convert.ToInt64(item),
                                    IsActive = true
                                };
                                db.wmpWebsitePagesLinkings.Add(_pageModel);
                                db.SaveChanges();
                            }
                            else
                            {
                                _model.IsActive = true;
                                db.Entry(_model).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                        }
                    }
            }
            return View(model);
        }

        private string uploadPostImage(HttpPostedFileBase txtfile)
        {
            if (txtfile != null)
            {
                string pic = System.IO.Path.GetFileName(txtfile.FileName);
                string _mainPath = Server.MapPath("~/Upload/postImages");
              
                if (!Directory.Exists(_mainPath+"/"+DateTime.Now.Year.ToString()))
                {
                    Directory.CreateDirectory(_mainPath + "/" + DateTime.Now.Year.ToString());
                    if (!Directory.Exists(_mainPath + "/" + DateTime.Now.Year.ToString()+"/"+DateTime.Now.Month.ToString()))
                    {
                        Directory.CreateDirectory(_mainPath + "/" + DateTime.Now.Year.ToString() + "/" + DateTime.Now.Month.ToString());
                    }
                }
                string path = System.IO.Path.Combine(Server.MapPath("~/Upload/postImages"+"/"+DateTime.Now.Year.ToString()+"/"+DateTime.Now.Month.ToString()), pic);
                // file is uploaded
                txtfile.SaveAs(path);
                // resizing image
                //WebImage img = new WebImage(path);

                //if (img.Width > 200)
                //    img.Resize(200, 200);
                //img.Save(path);

                // save the image path path to the database or you can send image 
                // directly to database
                // in-case if you want to store byte[] ie. for DB
                //using (MemoryStream ms = new MemoryStream())
                //{
                //    txtfile.InputStream.CopyTo(ms);
                //    byte[] array = ms.GetBuffer();
                //}
                return "/Upload/postImages" + "/" + DateTime.Now.Year.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + pic;
            }
            return "";
        }
        #region
        [HttpPost]
        public JsonResult getPagesLinkingCategoryTree(Int64 pageId)
        {
            JsTreeModel rootNode = new JsTreeModel();
            rootNode.attr = new JsTreeAttribute();
            rootNode.data = "Root";
            rootNode.attr.id = "0";
            //string rootPath = Request.MapPath("/Controllers");
            //rootNode.attr.id = rootPath;
            PopulateTree(0, rootNode, pageId);
            return Json(rootNode);
        }
        /// <summary>
        /// A method to populate a TreeView with directories, subdirectories, etc
        /// </summary>
        /// <param name="dir">The path of the directory</param>
        /// <param name="node">The "master" node, to populate</param>
        public void PopulateTree(Int64 parentID, JsTreeModel node,Int64 pageId)
        {
            if (node.children == null)
            {
                node.children = new List<JsTreeModel>();
            }


            var _list = db.Database.SqlQuery<TreeModel>("USP_getPagesLinkingCategoryTree @p0,@p1", parentID,pageId);

            int count = 0;
            // loop through each subdirectory
            foreach (var item in _list)
            {

                count = _list.Count();
                // create a new node
                JsTreeModel t = new JsTreeModel();
                t.attr = new JsTreeAttribute();
                t.attr.id = item.CategoryID.ToString();
                t.state = "disabled";
                if (item.IsActive)
                {
                    t.attr.Class = "jstree-checked";
                }
                t.data = item.CategoryName.ToString();
                
                // populate the new node recursively
                PopulateTree(item.CategoryID, t,pageId);
                node.children.Add(t); // add the node to the "master" node
            }

        }
        #endregion
    }
}
public class TreeModel
{
    public Int64 CategoryID { get; set; }
    public string CategoryName { get; set; }
    public bool IsActive { get; set; }
}