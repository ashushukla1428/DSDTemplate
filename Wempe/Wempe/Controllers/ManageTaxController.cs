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
    public class ManageTaxController : Controller
    {
        //
        // GET: /ManageTax/
        dbWempeEntities db = new dbWempeEntities();
        public ActionResult Index()
        {
            ViewBag.Countries = new SelectList(db.wmpCountries.Where(c => c.IsActive == true).OrderBy(c => c.country).Select(c => new { c.Id, c.country }), "Id", "country");
           // ViewBag.Sates = new SelectList(db.wmpStates.Where(c => c.IsActive == true && c.OwnerID == SessionMaster.Current.OwnerID).OrderBy(c => c.stateFullName).Select(c => new { c.Id, c.stateFullName }), "Id", "stateFullName");
            return View();
        }
        [HttpPost]
        public JsonResult Add(wmpTaxRate model)
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
                        if (db.wmpTaxRates.Any(c => c.taxRate == model.taxRate && c.OwnerID == model.OwnerID && c.StateId==model.StateId))
                        {
                            return Json(new Result { Status = false, Message = Messages.recordAlreadyExists }, JsonRequestBehavior.AllowGet);
                        }
                        db.wmpTaxRates.Add(model);
                    }
                    else
                    {
                        if (db.wmpTaxRates.Any(c => c.taxRate == model.taxRate && c.Id != model.Id && c.StateId == model.StateId && c.OwnerID == model.OwnerID))
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
        public JsonResult getList(SearchFilters model, int? CountryId, int? StateId, int? CityId, string TaxType)
        {
            try
            {
                var _items = db.Database.SqlQuery<TaxRateModel>("USP_GetTaxRate @p0, @p1, @p2, @p3, @p4, @p5,@p6,@p7,@p8,@p9", model.Name == null ? "" : model.Name, SessionMaster.Current.OwnerID, model.pageNo, Convert.ToInt32(MainSetting.pageSize), model.sortColumn, model.sortOrder, CountryId, StateId, CityId, TaxType);
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
            var _data = db.wmpTaxRates.Find(id);
            TaxRateModel _model = new TaxRateModel() { Id = _data.Id, taxRate = _data.taxRate, IsActive = _data.IsActive, StateId = _data.StateId };
            _model.state = db.wmpSampleCities.Find(_data.StateId).StateId.ToString();
            _model.country = db.wmpStates.Find(Convert.ToInt32(_model.state)).CountryId.ToString();
            return Json(_model, JsonRequestBehavior.AllowGet);
        }
    }
}
