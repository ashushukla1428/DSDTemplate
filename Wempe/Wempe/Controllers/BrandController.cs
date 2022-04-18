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
    public class BrandController : Controller
    {
        dbWempeEntities db = new dbWempeEntities();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Add(wmpBrandMaster model)
        {
            try
            {
                model.LastUpdate = DateTime.Now;
                model.UpdateBy = SessionMaster.Current.LoginId;
           
                if (ModelState.IsValid)
                {
                    if (model.BrandID == 0)
                    {
                        if (db.wmpBrandMasters.Any(c => c.BrandName == model.BrandName))
                        {
                            return Json(new Result { Status = false, Message = Messages.recordAlreadyExists }, JsonRequestBehavior.AllowGet);
                        }
                        model.OwnerID = SessionMaster.Current.OwnerID;
                        db.wmpBrandMasters.Add(model);
                    }
                    else
                    {
                        if (db.wmpBrandMasters.Any(c => c.BrandName == model.BrandName && c.BrandID != model.BrandID))
                        {
                            return Json(new Result { Status = false, Message = Messages.recordAlreadyExists }, JsonRequestBehavior.AllowGet);
                        }
                        model.OwnerID = SessionMaster.Current.OwnerID;
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
                var _items = db.Database.SqlQuery<BrandModel>("USP_GetBrand @p0, @p1, @p2, @p3, @p4, @p5, @p6", model.Name == null ? "" : model.Name, SessionMaster.Current.OwnerID, model.pageNo, Convert.ToInt32(MainSetting.pageSize), model.sortColumn, model.sortOrder, model.ActiveOrAllCheck);

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
            var _Brand = db.wmpBrandMasters.Find(id);
            BrandModel _model = new BrandModel() { brandId = _Brand.BrandID, BrandName = _Brand.BrandName, IsActive = _Brand.IsActive, Status = true };
            return Json(_model, JsonRequestBehavior.AllowGet);
        }
    }
}
