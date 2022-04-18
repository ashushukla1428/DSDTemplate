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
    public class ItemLocationController : Controller
    {
        //
        // GET: /ItemLocation/
        dbWempeEntities db = new dbWempeEntities();
        public ActionResult Index()
        {
            ViewBag.Countries = new SelectList(db.wmpCountries.Where(c => c.IsActive == true).OrderBy(c => c.country).Select(c => new { c.Id, c.country }), "Id", "country", 253);
            return View();
        }

        public ActionResult Add()
        {
            ViewBag.Countries = new SelectList(db.wmpCountries.Where(c => c.IsActive == true).OrderBy(c => c.country).Select(c => new { c.Id, c.country }), "Id", "country", 253);

            return View();
        }

        [HttpPost]
        public JsonResult Add(wmpSupplier model)
        {
            try
            {
                model.LastUpdate = DateTime.Now;
                model.UpdateBy = SessionMaster.Current.LoginId;
                model.OwnerID = SessionMaster.Current.OwnerID;

                if (ModelState.IsValid)
                {
                    if (model.Id == 0)
                    {
                        if (db.wmpSuppliers.Any(c => c.name == model.name && c.OwnerID == model.OwnerID))
                        {
                            return Json(new Result { Status = false, Message = Messages.recordAlreadyExists }, JsonRequestBehavior.AllowGet);
                        }
                        db.wmpSuppliers.Add(model);
                    }
                    else
                    {
                        if (db.wmpSuppliers.Any(c => c.name == model.name && c.Id != model.Id && c.OwnerID == model.OwnerID))
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
                var _items = db.Database.SqlQuery<ItemLocationModel>("USP_GetItemLocation @p0, @p1, @p2, @p3, @p4, @p5, @p6", model.Name == null ? "" : model.Name, SessionMaster.Current.OwnerID, model.pageNo, Convert.ToInt32(MainSetting.pageSize), model.sortColumn, model.sortOrder, model.ActiveOrAllCheck);

                return Json(_items, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }
        public ActionResult Edit(Int64 id)
        {
            return View();
        }
        [HttpGet]
        public JsonResult Edit(int id)
        {
            var _itme = db.wmpSuppliers.Find(id);
       //     wmpSupplier _model = new wmpSupplier() { BezelConditionID = _Bezel.BezelConditionID, BezelCondition = _Bezel.BezelCondition, IsActive = _Bezel.IsActive, Status = true };
            return Json(_itme, JsonRequestBehavior.AllowGet);
        }
        //get states
        [HttpGet]
        public JsonResult getStates(int Id)
        {
            try
            {
                //return Json(db.wmpStates.Where(c => c.IsActive == true && c.OwnerID == SessionMaster.Current.OwnerID && c.CountryId==Id).OrderBy(c => c.stateFullName).Select(c => new { c.Id, c.stateFullName }),JsonRequestBehavior.AllowGet);
                return Json(db.wmpStates.Where(c => c.IsActive == true && c.CountryId == Id).OrderBy(c => c.stateFullName).Select(c => new { c.state, c.stateFullName }), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(new Result { Status = false, Message = ex.Message });
            }
        }
        //getCities
        [HttpGet]
        public JsonResult getCities(string Id)
        {
            try
            {
                // return Json(db.wmpSampleCities.Where(c => c.IsActive == true && c.OwnerID == SessionMaster.Current.OwnerID && c.StateId == Id).OrderBy(c => c.city).Select(c => new { c.Id, c.city }), JsonRequestBehavior.AllowGet);
                var _stateData = db.wmpStates.Where(c => c.state == Id).FirstOrDefault();
                Int64 stateId = 0;
                if (_stateData != null) { stateId = _stateData.Id; }


                return Json(db.wmpSampleCities.Where(c => c.IsActive == true && c.StateId == stateId).OrderBy(c => c.city).Select(c => new { c.Id, c.city }), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(new Result { Status = false, Message = ex.Message });
            }
        }

    }
}
