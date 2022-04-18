using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wempe.CommonClasses;
using Wempe.Models;

namespace Wempe.Controllers
{
    public class MenuController : Controller
    {
        //
        // GET: /Menu/

        dbWempeEntities db = new dbWempeEntities();
        public const int PageSize = 10;
        //
        // GET: /Appraiser/

        public ActionResult Index()
        {
            var data = new PagedData<wmpMenuMaster>();

            var _menus=db.wmpMenuMasters.OrderByDescending(p => p.MenuName).Take(PageSize).ToList();
         
         

            data.Data =  db.wmpMenuMasters.OrderByDescending(p => p.MenuName).Take(PageSize).ToList();
            data.NumberOfPages = Convert.ToInt32(Math.Ceiling((double)db.wmpMenuMasters.Count() / PageSize));
            var _pageData = db.wmpWebsitePages.Where(c => c.IsActive == true).Select(c=>new{c.PageID,c.PageName});
            SelectList list = new SelectList(_pageData, "PageID", "PageName");
            ViewBag.pageData = list;
            
            return View(data);
        }
        [HttpGet]
        public ActionResult PageList(int page)
        {
            var data = new PagedData<wmpMenuMaster>();
            data.Data = db.wmpMenuMasters.OrderByDescending(p => p.MenuName).Skip(PageSize * (page - 1)).Take(PageSize).ToList();
            data.NumberOfPages = Convert.ToInt32(Math.Ceiling((double)db.wmpMenuMasters.Count() / PageSize));
            data.CurrentPage = page;

            return PartialView(data);
        }
        [HttpGet]
        public JsonResult Edit(int id)
        {
            var _data = db.wmpMenuMasters.Find(id);
            return Json(new { _data.MenuID, _data.MenuName, _data.IsActive, _data.PageName,_data.parentID,_data.PageID,_data.MenuIndex }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public JsonResult Add(wmpMenuMaster model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (db.wmpMenuMasters.Any(c => c.MenuName == model.MenuName && c.parentID==model.parentID))
                    {
                        return Json(new Result { Status = false, Message = Messages.recordAlreadyExists }, JsonRequestBehavior.AllowGet);
                    }

                    if (model.MenuID == 0)
                    {
                        db.wmpMenuMasters.Add(model);
                    }
                    else
                    {
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
                var data = db.wmpMenuMasters.Find(id);
                db.wmpMenuMasters.Remove(data);
                db.SaveChanges();
                return Json(new Result { Status = true, Message = Messages.recordDeleted }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(new Result { Status = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        
    }
}
