using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Wempe.CommonClasses;
using Wempe.Models;

namespace Wempe.Controllers
{
    [CustomAuthorize()]
    public class AppraiserController : Controller
    {
        dbWempeEntities db = new dbWempeEntities();
        public const int PageSize = 10;
        //
        // GET: /Appraiser/
     
        public ActionResult Index()
        {
            var model = db.wmpPrintSettings.Where(c => c.OwnerID == SessionMaster.Current.OwnerID).FirstOrDefault();

            return View(model);
        }

        [HttpPost]
        public JsonResult getList(SearchFilters model)
        {
            try
            {
                var _items = db.Database.SqlQuery<AppraiserModel>("USP_GetAppraiser @p0, @p1, @p2, @p3, @p4,@p5,@p6", model.Name == null ? "" : model.Name, model.pageNo, Convert.ToInt32(MainSetting.pageSize), model.sortColumn, model.sortOrder, SessionMaster.Current.OwnerID, model.ActiveOrAllCheck);
                
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
            var _Appraiser = db.wmpAppraiserMasters.Find(id);
            AppraiserModel _model = new AppraiserModel() {AppraiserID=_Appraiser.AppraiserID,AppraiserTitle=_Appraiser.AppraiserTitle,IsActive=_Appraiser.IsActive,Status=true };
            return Json(_model, JsonRequestBehavior.AllowGet); 
        }

        [HttpPost]
        public JsonResult Add(wmpAppraiserMaster model)
        {
            try
            {
                model.LastUpdate = DateTime.Now;
                model.UpdateBy = SessionMaster.Current.LoginId;
                model.OwnerID = SessionMaster.Current.OwnerID;
                if (ModelState.IsValid)
                {
                    if (model.AppraiserID == 0)
                    {
                        if (db.wmpAppraiserMasters.Any(c => c.AppraiserTitle == model.AppraiserTitle && c.OwnerID == model.OwnerID))
                        {
                            return Json(new Result { Status = false, Message = Messages.recordAlreadyExists }, JsonRequestBehavior.AllowGet);
                        }
                        db.wmpAppraiserMasters.Add(model);
                    }
                    else
                    {
                        if (db.wmpAppraiserMasters.Any(c => c.AppraiserTitle == model.AppraiserTitle && c.AppraiserID != model.AppraiserID && c.OwnerID == model.OwnerID))
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
    }
}
