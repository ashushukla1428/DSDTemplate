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
    [CustomAuthorize()]
    public class BackConditionController : Controller
    {
        dbWempeEntities db = new dbWempeEntities();

        public ActionResult Index()
        {
            ViewBag.Brand = new SelectList(db.wmpBrandMasters.Where(c => c.IsActive == true).OrderBy(c => c.BrandName).Select(c => new { c.BrandID, c.BrandName }), "BrandID", "BrandName");
            return View();
        }

        [HttpPost]
        public JsonResult Add(wmpBackConditionMaster model)
        {
            try
            {
                if (model.brandId == null)
                {
                    model.brandId = 0;
                }


                model.LastUpdate = DateTime.Now;
                model.UpdateBy = SessionMaster.Current.LoginId;
                model.OwnerID = SessionMaster.Current.OwnerID;
                if (ModelState.IsValid)
                {
                    if (model.BackConditionID == 0)
                    {
                        if (db.wmpBackConditionMasters.Any(c => c.BackCondition == model.BackCondition && c.OwnerID == model.OwnerID))
                        {
                            return Json(new Result { Status = false, Message = Messages.recordAlreadyExists }, JsonRequestBehavior.AllowGet);
                        }
                        
                        db.wmpBackConditionMasters.Add(model);
                    }
                    else
                    {
                        if (db.wmpBackConditionMasters.Any(c => c.BackCondition == model.BackCondition && c.BackConditionID != model.BackConditionID && c.OwnerID==model.OwnerID))
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
        public JsonResult getList(SearchFilters model)
        {
            try
            {
                var _items = db.Database.SqlQuery<BackConditionModel>("USP_GetBackCondition @p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7", model.Name == null ? "" : model.Name, SessionMaster.Current.OwnerID, model.pageNo, Convert.ToInt32(MainSetting.pageSize), model.sortColumn, model.sortOrder, model.BrandId, model.ActiveOrAllCheck);

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
            var _BackCon = db.wmpBackConditionMasters.Find(id);
            BackConditionModel _model = new BackConditionModel() { BackConditionID = _BackCon.BackConditionID, BackCondition = _BackCon.BackCondition, brandId=_BackCon.brandId, IsActive = _BackCon.IsActive, Status = true };
            return Json(_model, JsonRequestBehavior.AllowGet);
        }


    }
}
